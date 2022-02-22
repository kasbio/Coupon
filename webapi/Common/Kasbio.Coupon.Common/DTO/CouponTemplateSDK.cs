using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    public class CouponTemplateSDK
    {

        /// <summary>
        /// 优惠券模板主键
        /// </summary>
        public int Id{ set; get; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name{ set; get; }
        /// <summary>
        /// 优惠券Logo
        /// </summary>
        public string Logo{ set; get; }
        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string Desc{ set; get; }
        /// <summary>
        /// 优惠券分类
        /// </summary>
        public string Category{ set; get; }
        /// <summary>
        /// 产品线
        /// </summary>
        public int ProductLine{ set; get; }
        /// <summary>
        /// 优惠券模板的编码
        /// </summary>
        public string Key{ set; get; }
        /// <summary>
        /// 优惠券目标用户
        /// </summary>
        public int Target{ set; get; }
        /// <summary>
        /// 优惠券规则
        /// </summary>
        public TemplateRule Rule{ set; get; }


        public CouponTemplateSDK(int id,string name,string logo,string desc ,string category
                                ,int productLine,string key,int target, TemplateRule rule)
        {
            this.Id = id;
            this.Name = name;
            this.Logo = logo;
            this.Category = category;
            this.ProductLine = productLine;
            this.Key = key;
            this.Target = target;
            this.Rule = rule;
            this.Desc = desc;
        }

        public CouponTemplateSDK()
        {

        }
    }

}
