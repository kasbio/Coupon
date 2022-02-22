//using Coupon.Common.Extension.Json;
//using Coupon.Common.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Text.Json;
//using System.Text.Unicode;
//using System.Threading.Tasks;

//namespace Coupon.Template.Model.DTO
//{
//    public class TemplateRequest
//    {

//        /// <summary>
//        /// 优惠券名称
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 优惠券Logo
//        /// </summary>
//        public string Logo { get; set; }



//        /// <summary>
//        /// 优惠券描述
//        /// </summary>
//        public string Desc { get; set; }

//        /// <summary>
//        /// 优惠券分类
//        /// </summary>
//        public CouponCategory Category { get; set; }

//        /// <summary>
//        /// 产品线
//        /// </summary>
//        public ProductLine ProductLine { get; set; }

//        /// <summary>
//        /// 总数
//        /// </summary>
//        public int Count { get; set; }

//        /// <summary>
//        /// 创建用户
//        /// </summary>
//        public long UserId { get; set; }

//        /// <summary>
//        /// 目标用户
//        /// </summary>
//        public DistributeTarget Target { get; set; }

//        /// <summary>
//        /// 优惠券规则
//        /// </summary>
//        public TemplateRule Rule { get; set; }


//        /// <summary>
//        /// 校验对象的合法性
//        /// </summary>
//        /// <returns></returns>
//        public bool Validate()
//        {
//            bool stringValid = !string.IsNullOrEmpty(Name)
//                    && !string.IsNullOrEmpty(Logo)
//                    && !string.IsNullOrEmpty(Desc);
//            bool enumValid = Category != null
//                    && ProductLine != null
//                    && Target != null;
//            bool numValid = Count > 0 && UserId > 0;

//            return stringValid && enumValid && numValid && Rule.Validate();
//        }

//        public override string ToString()
//        {
//            return this.ToJson();
//        }
//    }
//}
