using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Extensions;
using Kasbio.Coupon.Settlement.ApplicationCore.Executor;
using Kasbio.Coupon.Settlement.ApplicationCore.Executor.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kasbio.Coupon.Settlement.Web
{
    public class Startup
    {

     

        public Startup(IConfiguration configuration )
        {
            Configuration = configuration;
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IRuleExecutor, LiJianExecutor>();
            services.AddSingleton<IRuleExecutor,ManjianExecutor>();
            services.AddSingleton<IRuleExecutor,ManJianZheKouExecutor>();
            services.AddSingleton<IRuleExecutor,ZheKouExecutor>();
            services.AddSingleton(typeof(ExecuteManager));
            services.ConfigureMvcOption();
            services.AddHealthChecks();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
       
            //获取注册的规则计算器
            ExecuteManager.Collect(app);
       
            app.UseHttpsRedirection();

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
            app.RegisterToConsul(Configuration, lifetime);
        }
    }
}
