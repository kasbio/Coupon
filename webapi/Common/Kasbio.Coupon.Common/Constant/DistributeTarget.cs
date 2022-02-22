using System.Collections.Generic;
using System.Linq;

namespace Kasbio.Coupon.Common.Constant
{
    public  class DistributeTarget
    {
        public static DistributeTarget SINGLE = new DistributeTarget("单用户", 1);
        public static DistributeTarget MULTI = new DistributeTarget("多用户", 2);

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

        DistributeTarget(string description, int code)
        {
            this.code = code;
            this.description = description;
        }
        public static DistributeTarget Find(int code)
        {
            return Values().FirstOrDefault(o => o.code == code);
        }
        static IEnumerable<DistributeTarget> Values()
        {
            yield return SINGLE;
            yield return MULTI;
        }
    }

}
