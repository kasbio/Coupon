using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    /// <summary>
    /// 折扣条件
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// 额度：满减(20),折扣（85），立减（10）
        /// </summary>
        public int Quota { get; set; }


        /// <summary>
        /// 基准，需要满多少才可用
        /// </summary>
        public int Base { get; set; }

        public bool Validate()
        {
            return Quota > 0 && Base > 0;
        }
    }
}
