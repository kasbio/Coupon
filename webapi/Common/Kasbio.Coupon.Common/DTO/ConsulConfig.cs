using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Common.DTO
{
    public class ConsulConfig
    {
        public string ServiceName { get; set; }

        public string Address { get; set; }

        public string Protocol { get; set; }

        public string ServerUrl { get; set; }
        public int Port { get; set; }

        public HealthConfg Health { get; set; }

        public string Url
        {
            get
            {
                return $"{ Protocol}://{Address}:{Port}";
                
            }
        }
    }

    public class HealthConfg
    {
        public int DeregisterCriticalServiceAfter { get; set; }

        public string Url { get; set; }

        public int Interval { get; set; }

        public int MyProperty { get; set; }

        public int TimeOut { get; set; }
    }
}
