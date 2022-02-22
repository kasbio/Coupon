using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Common.Utilities;
using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Utility
{
  public  class CouponClassify
    {
        public CouponClassify()
        {

        }
        public List<Entity.Coupon> Used { get; set; }

        public List<Entity.Coupon> Usable{ get; set; }

        public List<Entity.Coupon> Expired { get; set; }

        public static CouponClassify Classify(List<Entity.Coupon> coupons)
        {
            List<Entity.Coupon> used = new List<Entity.Coupon>();
            List<Entity.Coupon> usable = new List<Entity.Coupon>();
            List<Entity.Coupon> expired = new List<Entity.Coupon>();

            long curTime = DateTimeUtility.ConvertToTimeStamp(DateTime.Now);

            coupons.ForEach(s =>
            {
                bool isExpired = false;
                if (PeriodType.Find(s.TemplateSDK.Rule.Expiration.Period).Equals( PeriodType .SHIFT))
                {
                    isExpired =
                    DateTimeUtility.ConvertToTimeStamp(s.AssignTime.AddDays(s.TemplateSDK.Rule.Expiration.Gap))
                   < curTime;
                }
                else
                {
                    isExpired = s.TemplateSDK.Rule.Expiration.DeadLine < curTime;
                }

                if (isExpired || s.Status .Equals( CouponStatus.EXPIRED))
                {
                    s.Status = CouponStatus.EXPIRED;
                    expired.Add(s);
                }
                else
                {
                    if (s.Status.Equals(CouponStatus.USABLE))
                    {
                        usable.Add(s);
                    }
                    else
                    {
                        used.Add(s);
                    }
                }

            });


            return new CouponClassify()
            {
                Expired = expired,
                Usable = usable,
                Used = used
            };
        }
    }
}
