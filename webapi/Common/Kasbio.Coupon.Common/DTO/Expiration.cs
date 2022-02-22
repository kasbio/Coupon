using Kasbio.Coupon.Common.Constant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    /// <summary>
    /// 优惠券过期条件
    /// </summary>
    public class Expiration
    {
        /// <summary>
        /// 有限期规则，对应PeriodType 的code
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 有效间隔：只对变动性有效期有效 
        /// </summary>
        public int Gap { get; set; }

        /// <summary>
        /// 优惠券模板的失效日期，两类规则都有效
        /// </summary>
        public long DeadLine { get; set; }

        public bool Validate()
        {
            return null != PeriodType.Find(Period) && Gap > 0 && DeadLine > 0;
        }

        public Expiration(int period,int gap,long deadline)
        {
            this.Period = period;
            this.Gap = gap;
            this.DeadLine = deadline;
        }
        public Expiration()
        {

        }
    }
}
