using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Kasbio.Coupon.Common.Constant.Exception;

namespace Kasbio.Coupon.Common.Constant
{

    public class CouponCategory
    {
       public static CouponCategory MANJIAN = new CouponCategory("减满券", "001");

        public static CouponCategory ZHEKOU = new CouponCategory("折扣券", "002");

        public static CouponCategory LIJIAN = new CouponCategory("立减券", "003");


        CouponCategory(string description, string code)
        {
            this.description = description;
            this.code = code;
        }

       static IEnumerable<CouponCategory> Values()
        {
            yield return MANJIAN;
            yield return ZHEKOU;
            yield return LIJIAN;
        }


        private string description;
        /// <summary>
        /// 优惠券描述(分类)
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }



        

        private string code;
        /// <summary>
        /// 优惠券分类编码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }


 
 

        public static CouponCategory Find(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new CouponException($"CouponCategory.code:{code} : not exist!  ");
            }
            return Values().FirstOrDefault(o => o.code == code);
        }


        public  T SwitchAction<T>(Func<CouponCategory,T> func)
        {
            var list = Values();
            T o = default(T);
            foreach (var item in list)
            {
                if (item.Code == this.Code)
                {
                    o =  func(this);
                }
            }
            return o;
        }
    }
}
