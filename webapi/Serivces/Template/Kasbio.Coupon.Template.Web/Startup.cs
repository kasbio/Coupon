using Kasbio.Coupon.Common.Extensions;
using Kasbio.Coupon.Template.ApplicationCore.Extensions;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using Kasbio.Coupon.Template.ApplicationCore.Tasks;
using Kasbio.Coupon.Template.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kasbio.Coupon.Template.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureMvcOption();
            services.AddDbContext<TemplateDbContext>(option =>
            {
                
                option.UseMySql(Configuration.GetConnectionString("template"), b => b.MigrationsAssembly("Kasbio.Coupon.Template.Web"));
            }, ServiceLifetime.Singleton);

            //无法返回JsonResult
            //services.AddControllers().AddJsonOptions(o =>
            //{
            //    o.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            //});
            services.AddHealthChecks();
            // 3.0 存在System.Text.Json数组报错的问题
            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient<ITemplateRepository, TemplateRepository>();
            services.AddTemplateService();
            services.AddHostedService<CheckExpiredCouponTemplateTask>();

            var csredis = new CSRedis.CSRedisClient(Configuration.GetConnectionString("redis"));
            RedisHelper.Initialization(csredis);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/Health", new HealthCheckOptions()
            {
                ResultStatusCodes =
            {
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
            });
            //注册Consul
            app.RegisterToConsul(Configuration, lifetime);
        }
    }
}
