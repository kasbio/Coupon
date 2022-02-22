using Coupon.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Template.Grpc.Model;

namespace Coupon.Template.Model
{

    [Table("COUPON_TEMPLATE")]
   public class CouponTemplate
    {
        

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 是否可用状态
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 优惠券logo
        /// </summary>
        public string Logo { get; set; }


        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string Desc { get; set; }



        /// <summary>
        /// 优惠券分类
        /// </summary>
        public CouponCategory Category { get; set; }


        public ProductLine ProductLine { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        /// 
 
        public int Count { get; set; }


        /// <summary>
        /// 创建用户
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 优惠券模板的编码
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 目标用户
        /// </summary>
        public DistributeTarget Target { get; set; }

        /// <summary>
        /// 优惠券规则
        /// </summary>
        public TemplateRuleDTO Rule { get; set; }
        
        public DateTime CreateTime { get; set; }

        public CouponTemplate(string name, string logo, string desc, CouponCategory category,
              ProductLine productLine, int count, long userId,
              DistributeTarget target, TemplateRuleDTO rule)
        {

            
            this.Available = false;
            this.Expired = false;
            this.Name = name;
            this.Logo = logo;
            this.Desc = desc;
            this.Category = category;
            this.ProductLine = productLine;
            this.Count = count;
            this.UserId = userId;
            // 优惠券模板唯一编码 = 4(产品线和类型) + 8(日期: 20190101) + id(扩充为4位)
            this.Key = productLine.ToString() + category +
                    DateTime.Now.ToString("yyyyMMdd").ToString();
            this.Target =  target;
            this.Rule = rule;
            this.CreateTime = DateTime.Now;
        }

        public CouponTemplate()
        {

        }





    }
}
