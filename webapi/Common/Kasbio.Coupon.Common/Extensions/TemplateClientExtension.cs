using Kasbio.Coupon.Common.DTO;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Common.Extension
{
   public static  class TemplateClientExtension
    {
        public static IServiceCollection AddTemplateClient(this IServiceCollection services)
        {
            FallbackResponseMessage message = new FallbackResponseMessage(-1, "template request error", string.Empty);
            HttpResponseMessage m = new HttpResponseMessage();
            m.Content = new StringContent(JsonTool.Serialize(message));
         
            services.AddHttpClient("Template", o =>
            {
                o.BaseAddress = new Uri("http://localhost:9000/coupon-template/");

            }).AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().FallbackAsync(m, b =>
            {
                Console.WriteLine($"fallback here {b.Exception.Message}");
            
                return Task.FromResult(1);
            }));
            return services;
        }
    }
}
