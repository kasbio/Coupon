using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Constant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor.Impl
{
    public class ZheKouExecutor : BaseExecutor, IRuleExecutor
    {

        private readonly ILogger<ZheKouExecutor> _log;

        public ZheKouExecutor(ILogger<ZheKouExecutor> log)
        {
            this._log = log;
        }


        public SettlementInfo ComputeRule(SettlementInfo settlement)
        {
            double goodsSum = GoodsCostSum(settlement.GoodsInfos);
            SettlementInfo probability = ProcessGoodsTypeNotSatisfy(settlement, goodsSum);
            if (probability != null)
            {
                _log.LogDebug("ZheKou Template Is Not Match GoodsType!");
                return probability;
            }

            // 折扣优惠券可以直接使用, 没有门槛
            CouponTemplateSDK templateSDK = settlement.CouponAndTemplateInfos[0].Template;
            double quota = (double)templateSDK.Rule.Discount.Quota;

            // 计算使用优惠券之后的价格
            settlement.Cost = 
              (goodsSum * (quota * 1.0 / 100)) > MinCost ?(goodsSum * (quota * 1.0 / 100)): MinCost
            ;
            _log.LogDebug($"Use ZheKou Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");

            return settlement;
        }

        public RuleFlag GetRuleConfig()
        {
            return RuleFlag.ZHEKOU;
        }
    }
}
