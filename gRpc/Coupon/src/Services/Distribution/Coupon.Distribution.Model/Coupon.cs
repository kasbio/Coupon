using Coupon.Common.Model;
using Coupon.Distribution.Model.Contant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Coupon.Template.Grpc.Model;

namespace Coupon.Distribution.Model
{


    public class Coupon  
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public int TemplateId { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 优惠券码
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// 领取时间
        /// </summary>
 
        public DateTime AssignTime { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
      
        public CouponStatus Status { get; set; }


        public CouponTemplateDTO Template { get; set; }


        public Coupon()
        {

        }

        /// <summary>
        /// 返回一个无效的Coupon 对象
        /// </summary>
        /// <returns></returns>
        public static Coupon GetInvalidCoupon()
        {
            Coupon coupon = new Coupon();
            coupon.Id = -1;
            return coupon;
        }

        public Coupon(int templateId, long userId, string couponCode,
                      CouponStatus status)
        {
            this.TemplateId = templateId;
            this.UserId = userId;
            this.Status = status;
            this.CouponCode = couponCode;

        }

    }
}
