using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.Data
{
 public sealed   class DataBaseCheck
    {
        public static void CreateDbIfNotExists<T>(IHost host) where T: DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<T>();
                    bool isOk = context.Database.EnsureCreated();
                    Console.WriteLine(isOk.ToString());
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DataBaseCheck>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
