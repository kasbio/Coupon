using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Data;
using Kasbio.Coupon.Template.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kasbio.Coupon.Template.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).
                Build()
                ;

            DataBaseCheck.CreateDbIfNotExists<TemplateDbContext>(host);
            //CreateDbIfNotExists(host);
            host.Run();
        }


        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<TemplateDbContext>();
                    bool isOk = context.Database.EnsureCreated();
                    Console.WriteLine(isOk.ToString()) ;
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
