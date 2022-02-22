using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kasbio.Coupon.Common.Constant
{
    public class GoodsType
    {
        public static GoodsType WENYU = new GoodsType("文娱", 1);
        public static GoodsType SHENGXIAN = new GoodsType("生鲜", 2);
        public static GoodsType JIAJU = new GoodsType("家居", 3);
        public static GoodsType OTHERS = new GoodsType("其他", 4);

        private string description;

        public string Descrtiption
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

        GoodsType(string desc,int code)
        {
            this.description = desc;
            this.code = code;
        }

        static IEnumerable<GoodsType> Values()
        {
            yield return WENYU;
            yield return SHENGXIAN;
            yield return JIAJU;
            yield return OTHERS;
        }
        public static GoodsType Find(int code)
        {
            return Values().FirstOrDefault(o => o.code == code);
        }





    }
}
