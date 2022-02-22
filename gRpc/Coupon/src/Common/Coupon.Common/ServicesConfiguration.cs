using Microsoft.Extensions.Configuration;

namespace Coupon.Common;

public static class ServicesConfiguration
{
    public static ServiceConfigs Read(IConfiguration configuration)
    {
        ServiceConfigs config = new ServiceConfigs();
        configuration.GetSection("Services")?.Bind(config);
        return config;
    }
}


 

public class ServiceConfigs
{
    public string Template { get; set; }

    public string Distribution { get; set; }

    public string Settlement { get; set; }
}