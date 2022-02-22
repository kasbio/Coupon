using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Redis
{
    public  class RedisOption
    {
        public string IP { get; set; }

        public string Ports { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(IP))
            {
                throw new ArgumentNullException(nameof(IP));
            }

            if (string.IsNullOrEmpty(Ports))
            {
                return $"{IP}:6379";
            }

            return $"{IP}:{Ports}";

        }
    }
}
