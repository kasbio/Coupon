using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using Kasbio.Coupon.Template.ApplicationCore.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using System.Reflection;

namespace Kasbio.Coupon.Template.ApplicationCore.Extensions
{
    public static class TemplateExtension
    {

        public static IServiceCollection AddTemplateService(this IServiceCollection services)
        {
            services.AddSingleton<ITemplateService, TemplateService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;

        }
    }
}
