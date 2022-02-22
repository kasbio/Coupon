using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Common.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Converters;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kasbio.Coupon.Template.ApplicationCore.Entities
{
    [Table(name: "coupon_template")]
    [JsonConverter(typeof(TemplateJsonConverter))]
    public class CouponTemplate: BaseEntity
    {
        /// <summary>
        /// 是否可用状态
        /// </summary>
        public bool Available{get;set;}

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool Expired{get;set;}

        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name{get;set;}

        /// <summary>
        /// 优惠券logo
        /// </summary>
        public string Logo{get;set;}


        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string Desc{get;set;}

        /// <summary>
        /// 优惠券分类
        /// </summary>
        public CouponCategory Category{get;set;}

        /// <summary>
        /// 产品线
        /// </summary>
        [Column(name: "product_line")]
        public ProductLine ProductLine{get;set;}

        /// <summary>
        /// 总数
        /// </summary>
        [Column(name:"coupon_count")]
        public int Count{get;set;}

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(name:"create_time")]
        public DateTime CreateTime{get;set;}
        /// <summary>
        /// 创建用户
        /// </summary>
        [Column(name:"user_id")]
        public long UserId{get;set;}

        /// <summary>
        /// 优惠券模板的编码
        /// </summary>
        [Column(name: "template_key")]
        public string Key{get;set;}

        /// <summary>
        /// 目标用户
        /// </summary>
        public DistributeTarget Target{get;set;}
        /// <summary>
        /// 优惠券规则
        /// </summary>
        public TemplateRule Rule{get;set;}


        public CouponTemplate(string name, string logo, string desc, string category,
                      int productLine, int count, long userId,
                      int target, TemplateRule rule)
        {

            this.Available = false;
            this.Expired = false;
            this.Name = name;
            this.Logo = logo;
            this.Desc = desc;
            this.Category = CouponCategory.Find(category);
            this.ProductLine = ProductLine.Find(productLine);
            this.Count = count;
            this.UserId = userId;
            // 优惠券模板唯一编码 = 4(产品线和类型) + 8(日期: 20190101) + id(扩充为4位)
            this.Key = productLine.ToString() + category +
                    DateTime.Now.ToString("yyyyMMdd").ToString();
            this.Target = DistributeTarget.Find(target);
            this.Rule = rule;
        }


        public CouponTemplate()
        {

        }

     
    }
}
