using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Data;
using Kasbio.Coupon.Distribution.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kasbio.Coupon.Distribution.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            DataBaseCheck.CreateDbIfNotExists<CouponDbContext>(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
