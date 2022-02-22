using Coupon.Common;
using Coupon.Common.Model;
using Coupon.Distribution.Model.Contant;

namespace Coupon.Distribution.ApplicationCore.Utility;

public class CouponClassifyResult
{
    public CouponClassifyResult()
    {
    }

    /// <summary>
    /// 已用优惠券
    /// </summary>
    public List<Model.Coupon> Used { get; set; } = new List<Model.Coupon>();


    /// <summary>
    /// 可用优惠券
    /// </summary>
    public List<Model.Coupon> Usable { get; set; }= new List<Model.Coupon>();


    /// <summary>
    /// 过期优惠券
    /// </summary>
    public List<Model.Coupon> Expired { get; set; }= new List<Model.Coupon>();
}

public static class CouponClassify
{
    public static CouponClassifyResult Classify(List<Model.Coupon> coupons)
    {
        CouponClassifyResult result = new CouponClassifyResult();

        bool isExpired = false;
        foreach (var coupon in coupons)
        {
            
            if ((PeriodType)coupon.Template.Rule.Expiration.Period == PeriodType.SHIFT)
            {
                int gap = coupon.Template.Rule.Expiration.Gap;
                isExpired = coupon.AssignTime.AddDays(gap) < DateTime.Now;
            }
            else
            {
                isExpired = coupon.Template.Rule.Expiration.Deadline < DateTime.Now.ToTimeStamp();
            }

            if (isExpired  || coupon.Status == CouponStatus.EXPIRED)
            {
                coupon.Status = CouponStatus.EXPIRED;
                result.Expired.Add(coupon);
            }
            else
            {
                switch (coupon.Status)
                {
                    case CouponStatus.USED:
                        result.Used.Add(coupon);
                        break;
                    case  CouponStatus.USABLE:
                        result.Usable.Add(coupon);
                        break;
                    default:
                        break;
                }
            }
            
            
        }

        return result;
    }
}