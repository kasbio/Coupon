using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Common.Model
{
    [Flags]
    public enum CouponCategory
    {
        [Description("满减")]
        FullCut ,
        [Description("折扣")]
        Discount,
        [Description("立减")]
        Knock,

    }
}
