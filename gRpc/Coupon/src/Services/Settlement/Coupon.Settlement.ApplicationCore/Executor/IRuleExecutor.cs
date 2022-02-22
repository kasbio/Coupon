using Coupon.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Settlement.Grpc.Model;

namespace Coupon.Settlement.ApplicationCore.Executor
{
    internal interface IRuleExecutor
    {
        CouponCategory GetRuleConfig();


        SettlementInfos ComputeRule(SettlementInfos settlementInfo);
    }
}
