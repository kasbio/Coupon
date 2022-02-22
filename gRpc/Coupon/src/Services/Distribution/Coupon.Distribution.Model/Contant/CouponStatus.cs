using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Distribution.Model.Contant
{
    public enum CouponStatus
    {
        [Description("可用")]
        USABLE = 0,
        [Description("已使用")]
        USED = 1,
        [Description("过期")]
        EXPIRED = 2,
    }
}
