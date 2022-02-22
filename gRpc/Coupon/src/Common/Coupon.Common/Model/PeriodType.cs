using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Common.Model
{
    public enum PeriodType
    {
        [Description("固定的(固定日期)")]
        REGULAR =1 ,
        [Description("变动的(以领取之日开始计算)")]
        SHIFT =2



    }
}
