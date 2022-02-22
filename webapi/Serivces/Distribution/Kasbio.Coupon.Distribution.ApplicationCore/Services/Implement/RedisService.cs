using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using Kasbio.Coupon.Distribution.ApplicationCore.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using Kasbio.Coupon.Common;
using Kasbio.Coupon.Common.Constant.Exception;
using Newtonsoft.Json;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Services.Implement
{
    public class RedisService : IRedisService
    {
        private ILogger<RedisService> _logger;

        public RedisService(ILogger<RedisService> logger)
        {
            this._logger = logger;
        }

        public async Task<int> AddCouponToCacheAsync(long userId, List<Entity.Coupon> coupons, int status)
        {
            _logger.LogInformation($"userid :{userId} to save status[{status}] coupons:{JsonConvert.SerializeObject(coupons)} to cache" );
            int result = -1;
            CouponStatus couponStatus = CouponStatus.Find(status);

            if (couponStatus == CouponStatus.USABLE)
            {
                result =await AddCouponToCacheForUsableAsync(userId, coupons);
            }
            else if (couponStatus == CouponStatus.EXPIRED)
            {
                result = await AddCouponToCacheForExpiredAsync(userId, coupons);
            }
            else if (couponStatus == CouponStatus.USED)
            {
                result = await AddCouponToCacheForUsedAsync(userId, coupons);
            }



            return result;

        }

        private async Task<int> AddCouponToCacheForUsedAsync(long userId, List<Entity.Coupon> coupons)
        {
            try
            {
                _logger.LogInformation("add used coupons to cache");
                // 处理需要缓存的已使用优惠券
                Dictionary<string, string> needCacheObject =
                    coupons.ToDictionary(k => k.Id.ToString(), k => JsonConvert.SerializeObject(k));


                //获取已使用[Used],以及[Usable]可使用优惠券
                string redisKey_used = Status2RedisKey(CouponStatus.USED.Code, userId);
                string redisKey_uasble = Status2RedisKey(CouponStatus.USABLE.Code, userId);

                List<Entity.Coupon> curUsableCoupons =
                    await GetCacheCouponsAsync(userId, CouponStatus.USABLE.Code);

                if (curUsableCoupons.Count <= 1)
                {
                    throw new CouponException("No usable coupon");
                }

                List<int> curUsable = curUsableCoupons.Select(o => o.Id).ToList();
                //需要过期的id
                List<int> paramIds = coupons.Select(o => o.Id).ToList();

                if (!curUsable.Except(paramIds).Any())
                {
                    _logger.LogError("CurCoupons is Not Equal to cache");
                    throw new CouponException("CurCoupons is Not Equal to cache");
                }

                //删除需使用到hash key[Usable]
                var needCleanKey =
                    paramIds.Select(o => o.ToString()).ToArray();
                List<string> objs = new List<string>();
                foreach (var item in needCacheObject)
                {
                    objs.Add(item.Key);
                    objs.Add(item.Value);
                }

                var result =  RedisHelper.StartPipe(o =>
                {
                    var ret0 = o.HMSet(redisKey_used, objs.ToArray());
                    _logger.LogInformation($"Pipeline executed Result: {ret0.Counter}");
                    var ret = o.HDel(redisKey_uasble, needCleanKey);
                    Random random = new Random();
                    
                    //防止缓存雪崩
                    o.Expire(redisKey_used, random.Next(1, 5));
                    o.Expire(redisKey_uasble, random.Next(1, 5));

                });

                return coupons.Count;
            }
            catch (Exception)
            {

                throw;
            }
       
        }

        private async Task<int> AddCouponToCacheForExpiredAsync(long userId, List<Entity.Coupon> coupons)
        {
            try
            {
                _logger.LogInformation("add used coupons to cache");
                // 处理需要缓存的已使用优惠券
                Dictionary<string, string> needCacheObject =
                    coupons.ToDictionary(k => k.Id.ToString(), k => JsonConvert.SerializeObject(k));


                //获取已使用[EXPIRED],以及[Usable]可使用优惠券
                string redisKey_expired = Status2RedisKey(CouponStatus.EXPIRED.Code, userId);
                string redisKey_uasble = Status2RedisKey(CouponStatus.USABLE.Code, userId);

                List<Entity.Coupon> curUsableCoupons =
                    await GetCacheCouponsAsync(userId, CouponStatus.USABLE.Code);

                if (curUsableCoupons.Count <= 1)
                {
                    throw new CouponException("No usable coupon");
                }

                List<int> curUsable = curUsableCoupons.Select(o => o.Id).ToList();
                //需要过期的id
                List<int> paramIds = coupons.Select(o => o.Id).ToList();

                if (!curUsable.Except(paramIds).Any())
                {
                    _logger.LogError("CurCoupons is Not Equal to cache");
                    throw new CouponException("CurCoupons is Not Equal to cache");
                }

                //删除需使用到hash key[Usable]
                var needCleanKey =
                    paramIds.Select(o => o.ToString()).ToArray();
                List<string> objs = new List<string>();
                foreach (var item in needCacheObject)
                {
                    objs.Add(item.Key);
                    objs.Add(item.Value);
                }

                var result = RedisHelper.StartPipe(o =>
                {
                    var ret0 = o.HMSet(redisKey_expired, objs.ToArray());
                    _logger.LogInformation($"Pipeline executed Result: {ret0.Counter}");
                    var ret = o.HDel(redisKey_uasble, needCleanKey);
                    
                    Random random = new Random();

                    //防止缓存雪崩
                    o.Expire(redisKey_expired, random.Next(1, 5));
                    o.Expire(redisKey_uasble, random.Next(1, 5));

                });

                return coupons.Count;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<int> AddCouponToCacheForUsableAsync(long userId, List<Entity.Coupon> coupons)
        {
            _logger.LogDebug("add coupon for usable");
            Dictionary<string, string> needCachedObject = coupons.ToDictionary(o => o.Id.ToString(), o => JsonConvert.SerializeObject(o));
            

            string redisKey = Status2RedisKey(CouponStatus.USABLE.Code, userId);

            _logger.LogInformation($"Add {coupons.Count} Coupon to Cache: {userId},{redisKey}");

            List<string> objs = new List<string>();
            
            foreach (var item in needCachedObject)
            {
                objs.Add(item.Key);
                objs.Add(item.Value);
            }
            bool isOK =  await RedisHelper.HMSetAsync(redisKey, objs.ToArray());
            if (!isOK)
            {
                throw new Exception("");
            }

            Random random = new Random();

            await RedisHelper.ExpireAsync(redisKey, random.Next(1, 2) * 60 * 60);
            


            return coupons.Count;
        }

        public async Task<List<Entity.Coupon>> GetCacheCouponsAsync(long userId, int status)
        {
            _logger.LogInformation($"Get Coupon from Cache :userId:{userId} status:{status}");
            string redisKey = Status2RedisKey(status, userId);
            var hashSet = await RedisHelper.HGetAllAsync(redisKey);
            if (!hashSet.Keys.Any())
            {
                await SaveEmptyCouponListToCacheAsync(userId, status);
                return new List<Entity.Coupon>();
            }
     
            var result = hashSet.Select(o => JsonConvert.DeserializeObject<Entity.Coupon>(o.Value)).ToList();
            return result;
        }

        public async Task SaveEmptyCouponListToCacheAsync(long userId, params int[] status)
        {
            _logger.LogInformation($"save empty list to cache = for user {userId},status:{status}");
            foreach (var item in status)
            {
                string redisKey = Status2RedisKey(item, userId);
                await RedisHelper.HSetAsync(redisKey, "-1", JsonConvert.SerializeObject(Entity.Coupon.GetInvalidCoupon()));
            }

        }

        public SettlementInfo Settlement(SettlementInfo info)
        {
            throw new NotImplementedException();
        }

        public async Task<string> TryToAcquireCouponCodeFromCacheAsync(int templateId)
        {
            string redisKey = Constant.RedisPrefix.COUPON_TEMPLATE + templateId.ToString("D4");

            string couponCode = await RedisHelper.LPopAsync(redisKey);


            _logger.LogInformation($"Get CouponCode from Cache :{couponCode}");
            return couponCode;
        }


        private string Status2RedisKey(int status, long userId)
        {
            string redisKey = null;
            CouponStatus couponStatus = CouponStatus.Find(status);

            if (couponStatus == CouponStatus.USABLE)
            {
                redisKey = Constant.RedisPrefix.USER_COUPON_USABLE + userId;
            }
            else if (couponStatus == CouponStatus.EXPIRED)
            {
                redisKey = Constant.RedisPrefix.USER_COUPON_EXPIRED + userId;
            }
            else if (couponStatus == CouponStatus.USED)
            {
                redisKey = Constant.RedisPrefix.USER_COUPON_USED + userId;
            }


            return redisKey;


        }
    }
}
