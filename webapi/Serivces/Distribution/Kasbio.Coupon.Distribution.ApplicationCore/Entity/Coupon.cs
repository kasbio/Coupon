using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Common.Entities;
using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using Kasbio.Coupon.Distribution.ApplicationCore.Converter.JSON;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Entity
{
    [JsonConverter(typeof(CouponJsonConverter))]
    [Table("coupon")]
    public class Coupon:BaseEntity
    {
       
     


        /** 模板Id*/
        [Column("template_id")]
        public int TemplateId { get; set; }


        /** 用户ID*/
        [Column("user_id")]
        public long UserId { get; set; }

        /** 优惠券码 */
        [Column("coupon_code")]
        public string CouponCode { get; set; }

        /** 领取时间 */

        [Column("assign_time")]
        public DateTime AssignTime { get; set; }

        /** 优惠券状态*/
        [Column("status")]
        public CouponStatus Status { get; set; }


        public CouponTemplateSDK TemplateSDK { get; set; }


        public Coupon()
        {

        }

        /**
         *  <h2>返回一个无效的Coupon 对象</h2>
         * @return
         */
        public static Coupon GetInvalidCoupon()
        {
            Coupon coupon = new Coupon();
            coupon.Id=-1;
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
