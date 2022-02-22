using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Template.ApplicationCore.Extensions
{
   public static class RedisExtension
    {
        public static  IServiceCollection AddRedis(this IServiceCollection services,Action<RedisOption> options)
        {
            if (options == null)
            {
                throw new CouponException("error option[redis]");
            }
            RedisOption option = new RedisOption();
            options(option);

            var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379");
            RedisHelper.Initialization(csredis);
            return services;

        }


    }
}
