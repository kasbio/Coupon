using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Common
{
    public class CouponException : System.Exception
    {

        public CouponException(string message) : base(message)
        {

        }
    }
}
