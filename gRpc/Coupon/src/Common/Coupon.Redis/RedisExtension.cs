using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Coupon.Redis
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisOption> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            RedisOption option = new RedisOption();
            options(option);
            InitializeRedisClent(option.ToString());
            return services;

        }


        private static void InitializeRedisClent(string connection)
        {
            var csredis = new CSRedis.CSRedisClient(connection);
            RedisHelper.Initialization(csredis);
        }
        public static IServiceCollection AddRedis(this IServiceCollection services,IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _ = configuration.GetSection("RedisOption") 
                          ?? throw new KeyNotFoundException("no found section: RedisOptions");
            
            RedisOption option = new RedisOption();
            configuration.Bind("RedisOption",option);
            InitializeRedisClent(option.ToString());
            return services;
        }
        
        
    }
}
