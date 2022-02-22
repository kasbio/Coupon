using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    public class CouponAndTemplateInfo
    {
        /// <summary>
        /// Coupon 主键
        /// </summary>
        public int Id { get; set; }

        public CouponTemplateSDK Template { get; set; }
    }
}
