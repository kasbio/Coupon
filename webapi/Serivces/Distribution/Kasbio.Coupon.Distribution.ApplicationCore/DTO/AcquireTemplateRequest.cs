using Kasbio.Coupon.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Distribution.ApplicationCore.DTO
{
   public class AcquireTemplateRequest
    {
        public long UserId { get; set; }

        public CouponTemplateSDK TemplateSDK{ get; set; }
        public AcquireTemplateRequest()
        {

        }

        public AcquireTemplateRequest(long userId,CouponTemplateSDK templateSDK)
        {
            this.UserId = userId;
            this.TemplateSDK = templateSDK;
        }
    }
}
