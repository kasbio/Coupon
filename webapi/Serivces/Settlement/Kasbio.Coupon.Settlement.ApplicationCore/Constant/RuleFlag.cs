using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Constant
{
  public class RuleFlag
    {

        public static RuleFlag MANJIAN = new RuleFlag("满减券的计算规则");
        public static RuleFlag ZHEKOU = new RuleFlag("折扣券的计算规则");
        public static RuleFlag LIJIAN = new RuleFlag("立减券的计算规则");
        public static RuleFlag MANJIAN_ZHEKOU = new RuleFlag("满减券 + 折扣券的计算规则");
        public string Description { get; set; }


        RuleFlag(string description)
        {
            this.Description = description;
        }


    }
}
