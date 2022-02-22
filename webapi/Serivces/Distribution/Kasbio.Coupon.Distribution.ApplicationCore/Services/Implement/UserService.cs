using Kasbio.Coupon.Common.Clients;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using Kasbio.Coupon.Distribution.ApplicationCore.DTO;
using Kasbio.Coupon.Distribution.ApplicationCore.Interfaces;
using Kasbio.Coupon.Distribution.ApplicationCore.Services.Interface;
using Kasbio.Coupon.Distribution.ApplicationCore.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Extensions;
using Kasbio.Coupon.Common.Constant.Exception;


namespace Kasbio.Coupon.Distribution.ApplicationCore.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IRedisService _redisService;
        private readonly ILogger<UserService> _logger;
        private readonly ICouponRepository _repository;
        private readonly ITemplateClient _templateClient;
        private readonly ISettlementClient _settlementClient;

        public UserService(IRedisService service, ILogger<UserService> logger, ICouponRepository repository, ITemplateClient client, ISettlementClient settlementClient)
        {
            _redisService = service;
            _logger = logger;
            _repository = repository;
            _templateClient = client;
            _settlementClient = settlementClient;
        }

        /// <summary>
        /// 用户领取优惠券
        /// 1.判断用户对此模板的所拥有的优惠券是否已经达到最大领取数
        /// 2.新增
        /// 【Bug】无法感知是否过期
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Entity.Coupon> AcquireTemplateAsync(AcquireTemplateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request is null ");
            }

            //判断传入数据合法性
            Dictionary<int, CouponTemplateSDK> db_template =
                (await _templateClient.FindId2TemplateSdkAsync(request.TemplateSDK.Id)
                ).Data;


            if (!db_template.Any())
            {
                throw new CouponException("not exist templateId :" + request.TemplateSDK.Id);
            }
            CouponTemplateSDK curSdk = db_template[request.TemplateSDK.Id];
            if (curSdk.Rule.Expiration.DeadLine < DateTime.Now.ToTimeStamp())
            {
                throw new CouponException("coupon template is expired!");
            }

            //获取用户已有可用优惠券
            List<Entity.Coupon> userUsableCoupons = await FindCouponsByStatusAsync(request.UserId,
                                                CouponStatus.USABLE.Code);

            Dictionary<int, List<Entity.Coupon>> template2Coupon =
                userUsableCoupons
                .GroupBy(o => o.Id, (k, v) =>
                {
                    return new KeyValuePair<int, IEnumerable<Entity.Coupon>>(k, v);
                })
                .ToDictionary(o => o.Key, o => o.Value.ToList());



            if (template2Coupon.ContainsKey(curSdk.Id) &&
                template2Coupon[curSdk.Id].Count >= curSdk.Rule.Limitation)
            {
                _logger.LogError($"Exceed Template Assign Limitation: {request.TemplateSDK.Id}");
                throw new CouponException("Exceed Template Assign Limitation");
            }
            // 尝试去获取优惠券码
            String couponCode = await _redisService.TryToAcquireCouponCodeFromCacheAsync(
                    request.TemplateSDK.Id
            );
            if (string.IsNullOrEmpty(couponCode))
            {
                _logger.LogError($"Can Not Acquire Coupon Code: {request.TemplateSDK.Id}");
                throw new CouponException("Can Not Acquire Coupon Code");
            }

            Entity.Coupon newCoupon = new Entity.Coupon(
                    request.TemplateSDK.Id, request.UserId,
                    couponCode, CouponStatus.USABLE
            );
            newCoupon = await _repository.AddAsync(newCoupon);

            // 填充 Coupon 对象的 CouponTemplateSDK, 一定要在放入缓存之前去填充
            newCoupon.TemplateSDK = curSdk;
            List<Entity.Coupon> temp = new List<Entity.Coupon>();
            temp.Add(newCoupon);
            // 放入缓存中
            await _redisService.AddCouponToCacheAsync(
                    request.UserId,
                    temp,
                    CouponStatus.USABLE.Code
            );

            return newCoupon;
        }



        public async Task<List<CouponTemplateSDK>> FindAvailableTemplateAsync(long userId)
        {
            //获取所有可用优惠券模板
            List<CouponTemplateSDK> sdks = (await _templateClient.FindAllUsableTemplateAsync()).Data;
            long curTime = DateTime.Now.ToTimeStamp();
            //简单只是筛选未过期的模板，假如有其他定制，可以更深层使用
            sdks = sdks.Where(o => o.Rule.Expiration.DeadLine > curTime).ToList();


            //id: template id
            // left : 限制数，right: 模板sdk
            Dictionary<int, KeyValuePair<int, CouponTemplateSDK>> limit2Template = new Dictionary<int, KeyValuePair<int, CouponTemplateSDK>>();

            sdks.ForEach(o =>
            {
                limit2Template.Add(o.Id, new KeyValuePair<int, CouponTemplateSDK>(o.Rule.Limitation, o));
            });

            List<CouponTemplateSDK> result = new List<CouponTemplateSDK>();

            //获取用户可用优惠券
            List<Entity.Coupon> userUasbleCoupons =
                    await FindCouponsByStatusAsync(userId, CouponStatus.USABLE.Code);

            // 整理可用优惠券，每个优惠券模板下有多少张优惠券
            Dictionary<int, List<Entity.Coupon>> templateId2Coupons = userUasbleCoupons

                .GroupBy(o => o.TemplateId, (k, v) =>
                {
                    return new KeyValuePair<int, IEnumerable<Entity.Coupon>>(k, v);
                })
                .ToDictionary(o => o.Key, o => o.Value.ToList());

            foreach (var item in limit2Template)
            {
                int limit = item.Value.Key;
                CouponTemplateSDK sdk = item.Value.Value;
                if (templateId2Coupons.ContainsKey(sdk.Id) &&
                       templateId2Coupons[sdk.Id].Count >= limit)
                {
                    _logger.LogInformation("user :{userId} got template '{sdk.Id}' is bigger thant limit :{limit}");
                    continue;
                }
                result.Add(sdk);
            }
            return result;
        }

        /// <summary>
        /// 根据用户 id 和状态查询优惠券记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<Entity.Coupon>> FindCouponsByStatusAsync(long userId, int status)
        {
            //从redis中获取coupons
            List<Entity.Coupon> cacheCoupons = await _redisService.GetCacheCouponsAsync(userId, status);
            List<Entity.Coupon> result = new List<Entity.Coupon>();
            //假如没有则从数据库中获取
            if (cacheCoupons == null || !cacheCoupons.Any())
            {
                _logger.LogInformation("Coupons form Cache is empty");
                List<Entity.Coupon> dbCoupons = await _repository
                                    .GetAllByUserIdAndStatusAsync(userId, CouponStatus.Find(status));
                if (dbCoupons != null && dbCoupons.Any())
                {
                    _logger.LogInformation("Coupons from Db is not empty");
                    result = dbCoupons;
                    await _redisService.AddCouponToCacheAsync(userId, dbCoupons, status);
                }
                else
                {
                    _logger.LogInformation("Coupons from Db is empty");
                    return dbCoupons;
                }
            }
            else
            {
                result = cacheCoupons;
            }
            //剔除没有-1空优惠券
            result.RemoveAll(o => o.Id == -1);

            //没有直接返回空
            if (!result.Any())
            {
                return result;
            }

            //补齐Template信息
            List<int> ids =
                result.Select(o => o.TemplateId).ToList();

            Dictionary<int, CouponTemplateSDK> sdks =
                (await _templateClient
                        .FindId2TemplateSdkAsync(ids.ToArray())
                        ).Data;

            result.ForEach(o =>
            {
                o.TemplateSDK = sdks[o.TemplateId];
            });


            //可用状态下需要判断是否已经过期，并延迟处理
            if (CouponStatus.Find(status) == CouponStatus.USABLE)
            {
                CouponClassify cResult = CouponClassify.Classify(result);
                if (cResult != null && cResult.Expired.Any())
                {
                    _logger.LogInformation("Coupons has some expired...");
                    await _redisService.AddCouponToCacheAsync(userId,
                            cResult.Expired,
                            CouponStatus.EXPIRED.Code);
                    await _repository.UpdateRangeAsync(cResult.Expired.ToArray());
                }
                return cResult.Usable;
            }

            return result;
        }

        public async Task<SettlementInfo> SettlementAsync(SettlementInfo info)
        {
            List<CouponAndTemplateInfo> ctInfo =
              info.CouponAndTemplateInfos;

            List<Entity.Coupon> preSettleCoupon = new List<Entity.Coupon>();

            if (!ctInfo.Any())
            {
                //没有优惠券，直接算总价
                double goodsSum = 0.0;
                foreach (var gi in info.GoodsInfos)
                {
                    goodsSum += (gi.Price + gi.Count);
                }

                info.Cost = goodsSum;
            }
            else
            {
                //校验优惠券是否为用户所拥有
                List<Entity.Coupon> uCoupons =
                    await FindCouponsByStatusAsync(info.UserId, CouponStatus.USABLE.Code);

                Dictionary<int, Entity.Coupon> id2Coupons =
                    uCoupons.ToDictionary(o => o.Id, o => o);




                if (!id2Coupons.Any() || !id2Coupons.Keys.Except(ctInfo.Select(o => o.Id)).Any())
                {

                    _logger.LogInformation(string.Join(',', id2Coupons.Keys));
                    _logger.LogError("User Coupon has some problem,it is invalidate coupon!");
                    throw new CouponException("User Coupon has some problem,it is invalidate coupon!");
                }


                List<Entity.Coupon> settleCoupon = new List<Entity.Coupon>();
                ctInfo.ForEach(c =>
                {
                    settleCoupon.Add(id2Coupons[c.Id]);
                });

                preSettleCoupon = settleCoupon;
            }


            //往 settlementInfo发送计算
            SettlementInfo processedInfo =
                  (await _settlementClient.ComputeRuleAsync(info)).Data;


            //是否为核销 && 调用成功【优惠券不为空】
            if (processedInfo.Employ
                    && processedInfo.CouponAndTemplateInfos.Any())
            {
                await _redisService.AddCouponToCacheAsync(
                    info.UserId,
                    preSettleCoupon,
                    CouponStatus.USED.Code);
                // 更新 db
                preSettleCoupon.ForEach(o => {
                    o.Status = CouponStatus.USED;
                });
                await _repository.UpdateRangeAsync(preSettleCoupon.ToArray());

                return new SettlementInfo();

            }
            return processedInfo;

        }
    }
}
