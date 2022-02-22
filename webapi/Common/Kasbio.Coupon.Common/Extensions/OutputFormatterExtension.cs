using Kasbio.Coupon.Common.Advice;
using Kasbio.Coupon.Common.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Kasbio.Coupon.Common.Extensions
{
  public static  class OutputFormatterExtension
    {
        public static IServiceCollection ConfigureMvcOption(this IServiceCollection services)
        {
            services.Configure<MvcOptions>((o) =>
            { 
                o.OutputFormatters.Add(new CustomResponseFormatter());
                o.Filters.Add(new GobalExceptionFilter());
            });

            return services;
        }

    }
}
