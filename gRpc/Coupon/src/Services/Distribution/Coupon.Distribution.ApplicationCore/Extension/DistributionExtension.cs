using Coupon.Template.Grpc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Common;
using Coupon.Distribution.ApplicationCore.Services;
using Coupon.Distribution.ApplicationCore.Services.Implement;
using Coupon.Settlement.Grpc;
using Microsoft.Extensions.Configuration;

namespace Coupon.Distribution.ApplicationCore.Extension
{
    public static class DistributionExtension
    {
        public static IServiceCollection AddDistributionService(this IServiceCollection services,IConfiguration configuration)
        {
            var clients = ServicesConfiguration.Read(configuration);
            services.AddGrpcClient<CouponTemplateServices.CouponTemplateServicesClient>(o =>
            {
                o.Address = new Uri(clients.Template); 
            });

            services.AddGrpcClient<SettlementServices.SettlementServicesClient>(o =>
            {
                o.Address = new Uri(clients.Settlement);
            });
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
        
    }
}
