using Consul;
using Kasbio.Coupon.Common.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Kasbio.Coupon.Common.Extensions
{
    public static class ConsultExtension
    {
        public static  void RegisterToConsul(this IApplicationBuilder app, IConfiguration configuration, IApplicationLifetime lifetime)
        {
            var log = app.ApplicationServices.GetService(typeof(ConsulConfig)) as ILogger<ConsulConfig>;
            ConsulConfig consulConfig = new ConsulConfig();
                  configuration .Bind("Consul", consulConfig);
            if (consulConfig ==null)
            {
                log.LogError($"appsetting.json doesnt have 'Consul' section");
                return;    
            }

            if (string.IsNullOrEmpty(consulConfig.Protocol) || 
                consulConfig.Port<0 || 
                string.IsNullOrEmpty(consulConfig.ServiceName) ||
                string.IsNullOrEmpty(consulConfig.Address) )
            {
                log.LogError($"appsetting.json doesnt have 'Consul' section");
                return;
            }



            lifetime.ApplicationStarted.Register(() =>
            {
                //初始化ConsulClient
                var consulClient = new ConsulClient(ConsulClientConfiguration =>
                                ConsulClientConfiguration.Address = new Uri(consulConfig.ServerUrl)
                                );
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulConfig.Health.DeregisterCriticalServiceAfter),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(consulConfig.Health.Interval),
                    HTTP = consulConfig.Health.Url,
                    Timeout = TimeSpan.FromSeconds(consulConfig.Health.TimeOut)
                };
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    ID = Guid.NewGuid().ToString(),
                    Name = consulConfig.ServiceName,
                    Address = consulConfig.Address,
                    Port = consulConfig.Port,
                    Meta = new Dictionary<string, string>() { ["Protocol"] = consulConfig.Protocol },
                    Tags = new[] { "http" }
                };
               var result = consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
                
                lifetime.ApplicationStopped.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();

                });

            });

        }

    }
}
