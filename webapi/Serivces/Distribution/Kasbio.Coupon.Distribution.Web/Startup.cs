using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Clients;
using Kasbio.Coupon.Common.Clients.Impl;
using Kasbio.Coupon.Common.Extension;
using Kasbio.Coupon.Common.Extensions;
using Kasbio.Coupon.Distribution.ApplicationCore.Interfaces;
using Kasbio.Coupon.Distribution.ApplicationCore.Services.Implement;
using Kasbio.Coupon.Distribution.ApplicationCore.Services.Interface;
using Kasbio.Coupon.Distribution.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Kasbio.Coupon.Distribution.Web
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
            services.AddControllers();
            services.AddDbContext<CouponDbContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("coupon"));

            }, ServiceLifetime.Scoped);
            var csredis = new CSRedis.CSRedisClient(Configuration.GetConnectionString("redis"));
            RedisHelper.Initialization(csredis);
            services.AddHealthChecks();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITemplateClient, TemplateClient>();
            services.AddScoped<ISettlementClient, SettlementClient>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddTemplateClient();
            services.AddSettlementClient();
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
            app.UseHealthChecks("/Health", new HealthCheckOptions()
            {
                ResultStatusCodes =
            {
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.RegisterToConsul(Configuration, lifetime);
        }
    }
}
