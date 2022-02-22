using Kasbio.Coupon.Common.Constant.Exception;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Contant
{
   public class CouponStatus
    {
        public CouponStatus()
        {

        }

        CouponStatus(string desc, int code)
        {
            this.description = desc;
            this.code = code;
        }

        public static  CouponStatus USABLE = new CouponStatus("可用", 1);
        public static CouponStatus USED = new CouponStatus("可使用", 2);
        public static CouponStatus EXPIRED = new CouponStatus("过期", 3);




        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        private int code;

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public static IEnumerable<CouponStatus> Values()
        {
            yield return USABLE;
            yield return USED;
            yield return EXPIRED;
        }



        public static CouponStatus Find(int code)
        {
            /** 判断是否为空 */
            if (code < 0)
            {
                throw new ArgumentException($"Illegal Argument: CODE:{code}");
            }
            var result =  Values().FirstOrDefault(o => o.code == code);
            if (result == null )
            {
                throw new CouponException($"CouponStatus not exist code{code}");
            }
            return result;
        }
        public override bool Equals(object obj)
        {
            if (obj is CouponStatus)
            {
                var t = obj as CouponStatus;
                return t.Code == this.Code;
            }
            return false;
        }

    }
}
