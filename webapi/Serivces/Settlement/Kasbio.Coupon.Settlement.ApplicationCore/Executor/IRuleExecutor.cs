using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Constant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor
{
   public interface IRuleExecutor
    {
        RuleFlag GetRuleConfig();


        SettlementInfo ComputeRule(SettlementInfo settlementInfo);

    }
}
