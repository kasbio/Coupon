using AutoMapper;
using Coupon.Common;
using Coupon.Common.Model;
using Coupon.Distribution.Grpc.Model;
using Coupon.Distribution.Infrastructure;
using Coupon.Distribution.Model.Contant;
using Coupon.Distribution.Model.DTO;
using Coupon.Settlement.Grpc.Model;
using Coupon.Template.Grpc;
using Coupon.Template.Grpc.Model;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Distribution.DbContexts;
using static Coupon.Template.Grpc.CouponTemplateServices;
using Coupon.Distribution.Infrastructure.Repository;
using Coupon.Settlement.Grpc;

namespace Coupon.Distribution.ApplicationCore.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IUserService>? _logger;
        private readonly IRedisService _redisService;
        private readonly CouponTemplateServices.CouponTemplateServicesClient _templateClient;
        private readonly CouponDbContext _dbContext;
        private readonly SettlementServices.SettlementServicesClient _settlementClient;

        public UserService(ILogger<IUserService>? logger,
            CouponTemplateServicesClient templateClient,
            IRedisService redisService, CouponDbContext dbContext, 
            IMapper mapper, SettlementServices.SettlementServicesClient servicesClient)
        {
            _logger = logger;
            _templateClient = templateClient;
            _redisService = redisService;
            _dbContext = dbContext;
            _mapper = mapper;
            _settlementClient = servicesClient;
        }

        

        /// <summary>
        /// 用户领取优惠券
        /// 1.判断优惠券模板是否可用，是否过期，数量是否还存有
        /// 2.判断用户是否已经领取过
        /// 3.新增至redis/数据库保存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CouponDTO> AcquireAsync(AcquireRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            //检查优惠券模板是否可用，是否过期
            var p = new CouponTemplateId()
            {
                Id = request.TemplateId
            };
            var r = await _templateClient.InfosAsync(p);

            if (DateTime.Now.ToTimeStamp() > r.Rule.Expiration.Deadline || !r.Available)
            {
                throw new CouponException("优惠券不可用或已过期");
            }

            var usableSet = (await _redisService.GetCacheCouponsAsync(request.UserId, CouponStatus.USABLE))
                .Where(o => o.TemplateId == request.TemplateId);
            if (usableSet is null || !usableSet.Any())
            {
                usableSet = await _dbContext.Coupons.AsNoTracking()
                     .Where(o => o.Status == CouponStatus.USABLE)
                     .Where(o => o.TemplateId == request.TemplateId)
                     .ToListAsync();
            }
            if (usableSet is not null && usableSet.Count() >= r.Rule.Limitation)
            {
                throw new CouponException("优惠券已领，请勿重复领取");
            }
            string couponNo = await _redisService.TryToAcquireCouponCodeFromCacheAsync(
                   request.TemplateId
            );
            if (string.IsNullOrEmpty(couponNo))
            {
                throw new CouponException("优惠券余量已无，请期待下次领取");
            }

            Model.Coupon coupon = new Model.Coupon(request.TemplateId, request.UserId, couponNo, CouponStatus.USABLE);
            coupon.Template = r;
            coupon.AssignTime = DateTime.Now;
            await _dbContext.Coupons.AddAsync(coupon);
            await _dbContext.SaveChangesAsync();
            await _redisService.AddCouponToCacheAsync(request.UserId, new List<Model.Coupon> { coupon }, CouponStatus.USABLE);
            return _mapper.Map<CouponDTO>(coupon); //TODO:记得mapper里新增配置
        }


      
 

        public async Task<CouponTemplateList> FindAvailableTemplateAsync(UserIdRequest request)
        {
            if (request is null)
            {
                throw new CouponException("无效参数");
            }

            var usable = await FindCouponsByStatusAsync(new FindCouponsByStatusRequest()
            {
                Status =(int)CouponStatus.USABLE,
                UserId = request.UserId
            });
            if (usable.Coupon is null)
            {
                return null;
            }

            var templateIds = usable.Coupon.Select(x => x.Templateid).ToArray();

            CouponTemplateIds query = new CouponTemplateIds();
            query.Ids.AddRange(templateIds);

            var template = await _templateClient.FindId2TemplateSDKAsync(query);
            CouponTemplateList result = new CouponTemplateList();

            foreach (var ct in result.Data)
            {
                var count = usable.Coupon.Count(o => o.Templateid == ct.Id);
                var limitation = ct.Rule.Limitation;
                if (count >= ct.Rule.Limitation)
                {
                    _logger.LogInformation($"user :{request.UserId} got template '{ct.Id}' is bigger thant limit :{limitation}");
                    continue;
                }

                result.Data.Add(ct);
            }

            return result;

        }

        public async Task<CouponCollection> FindCouponsByStatusAsync(FindCouponsByStatusRequest request)
        {
            List<Model.Coupon> coupons = 
                await _redisService.GetCacheCouponsAsync(request.UserId,(CouponStatus)request.Status);
            List<Model.Coupon> result = coupons;
            CouponCollection c = new CouponCollection();

            var userId = request.UserId;
            var status = (CouponStatus)request.Status;


            if (coupons is null)
            {
                _logger.LogInformation("Coupons form Cache is empty");
                List<Model.Coupon> dbCoupons = await _dbContext.GetAllByUserIdAndStatusAsync(userId, status);
                if (dbCoupons != null && dbCoupons.Any())
                {
                    _logger.LogInformation("Coupons from Db is not empty");
                    result = dbCoupons;
                    await _redisService.AddCouponToCacheAsync(userId, dbCoupons, status);
                }
                else
                {
                    _logger.LogInformation("Coupons from Db is empty");
                    c.Coupon.AddRange(_mapper.Map<IEnumerable<CouponDTO>>(dbCoupons));
                    return c;
                }
            }

            //剔除没有-1空优惠券[缓存穿透]
            result.RemoveAll(o => o.Id == -1);
            //没有直接返回空
            if (!result.Any())
            {
                return c;
            }

            var templateIds = new CouponTemplateIds();
            templateIds.Ids.AddRange(result.Select(o => o.Id));
            var templates =await  _templateClient.FindId2TemplateSDKAsync(templateIds);

            var r = _mapper.Map<IEnumerable<CouponDTO>>(result);
            foreach (var item in r)
            {
                var t = templates.Data.FirstOrDefault(o => o.Id == item.Templateid);
                item.Template = t;
            }
            c.Coupon.AddRange(r);
            return c;
        }

        public async Task<SettlementInfos> SettlementAsync(SettlementInfos info)
        {
            var ctInfo = info.CouponInfos;
            IEnumerable<Model.Coupon> preSettleCoupon =Enumerable.Empty<Model.Coupon>();

            if (ctInfo is null || !ctInfo.Any())
            {
                double goodsSum  = info.GoodsInfos.Sum(o => o.Price * o.Count);
                info.Cost = goodsSum;
            }
            else
            {
                //检验优惠券是否为用户所拥有
                var couponResults =  await FindCouponsByStatusAsync(new FindCouponsByStatusRequest()
                {
                    UserId = info.UserId,
                    Status = (int) CouponStatus.USABLE
                });

                var coupons = couponResults.Coupon;

                var cids = ctInfo.Select(o => o.Id);
                var usebleCoupons =  couponResults.Coupon.Where(o => cids.Contains(o.Id));
                if (cids.Count() != usebleCoupons.Count())
                {
                    _logger.LogWarning("User Coupon has some problem,it  is invalidate coupon!");
                    throw new CouponException("User Coupon has some problem,it  is invalidate coupon!");
                }
                
                preSettleCoupon = _mapper.Map<IEnumerable<Model.Coupon>>(usebleCoupons);;
            }

            var processInfos =  await _settlementClient.ComputeRuleAsync(info);

            if (processInfos.Employ && processInfos.CouponInfos.Any())
            {
                await _redisService.AddCouponToCacheAsync(
                        info.UserId,
                        preSettleCoupon,
                        CouponStatus.USED);

                foreach (var coupon in preSettleCoupon)
                {
                    coupon.Status = CouponStatus.USED;
                }

                _dbContext.UpdateRange(preSettleCoupon.ToArray());

                await _dbContext.SaveChangesAsync();
              
            }
            
            return processInfos;
        }
    }
}
