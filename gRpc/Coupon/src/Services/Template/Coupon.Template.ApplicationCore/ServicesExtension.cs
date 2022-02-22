using Coupon.Template.ApplicationCore.Implement;
using Coupon.Template.ApplicationCore.Services;
using Coupon.Template.Infrastructure.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Coupon.Template.ApplicationCore;

public static class ServicesExtension
{
    public static void AddTemplateService(this IServiceCollection services)
    {
        services.AddScoped<ITemplateService, TemplateService>();
        //services.AddHostedService<CheckExpiredCouponTemplateTask>();
    }
}