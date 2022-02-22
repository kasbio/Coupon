using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kasbio.Coupon.Common.Constant
{
   public class ProductLine
    {
        public static ProductLine DAMAO = new ProductLine("大猫", 1);

        public static ProductLine DABAO = new ProductLine("大宝",2);

        ProductLine(string description,int code)
        {
            this.description = description;
            this.code = code;
        }

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



 

        static IEnumerable<ProductLine> Values()
        {
            yield return DAMAO;
            yield return DABAO;
        }

        public static ProductLine Find(int code)
        {
            return Values().FirstOrDefault(o => o.code == code);
        }

    }
}
