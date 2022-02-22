using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kasbio.Coupon.Common.Constant
{
    public class PeriodType
    {
        public static PeriodType REGULAR = new PeriodType("固定的(固定日期)", 1);
        public static PeriodType SHIFT = new PeriodType("变动的(以领取之日开始计算)", 2);

        PeriodType(string description, int code)
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

        static IEnumerable<PeriodType> Values()
        {
            yield return REGULAR;
            yield return SHIFT;

        }

        public static PeriodType Find(int code)
        {
            return Values().FirstOrDefault(o => o.code == code);

        }

        public override bool Equals(object obj)
        {
            if (obj is PeriodType)
            {
                var t = obj as PeriodType;
                return t.Code == this.Code;
            }
            return false;
        }

    }
}
