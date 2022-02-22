using Coupon.Settlement.ApplicationCore.Executor;
using Coupon.Settlement.ApplicationCore.Executor.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Common;
using Coupon.Template.Grpc;
using Microsoft.Extensions.Configuration;

namespace Coupon.Settlement.ApplicationCore
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddSettlementService(this IServiceCollection services,IConfiguration configuration)
        {   
            var clients = ServicesConfiguration.Read(configuration);
            services.AddSingleton<IRuleExecutor, KnockExecutor>();
            services.AddSingleton<IRuleExecutor, FullCutExecutor>();
            services.AddSingleton<IRuleExecutor, DiscountExecutor>();
            services.AddSingleton<IRuleExecutor, FullCutAndDiscountExecutor>();
            services.AddSingleton(s => {
                var l = s.GetService<ILogger<RuleExecutor>>();
                RuleExecutor executor = new RuleExecutor(l);
                executor.Collect(s);
                return executor;
            });
            services.AddGrpcClient<CouponTemplateServices.CouponTemplateServicesClient>(o =>
            {
                o.Address = new Uri(clients.Template); 
            });

            return services;
        }


    }
}
