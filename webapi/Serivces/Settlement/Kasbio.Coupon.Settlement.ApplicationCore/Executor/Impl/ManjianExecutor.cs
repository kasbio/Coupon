using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Constant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor.Impl
{
  public  class ManjianExecutor : BaseExecutor, IRuleExecutor
    {

        private readonly ILogger<ManjianExecutor> _log;

        public ManjianExecutor(ILogger<ManjianExecutor> log)
        {
            this._log = log;
        }

        public SettlementInfo ComputeRule(SettlementInfo settlement)
        {
            double goodsSum =  
             GoodsCostSum(settlement.GoodsInfos);
            SettlementInfo probability = ProcessGoodsTypeNotSatisfy(
                    settlement, goodsSum
            );
            if (probability != null)
            {
                _log.LogDebug("ManJian Template Is Not Match To GoodsType!");
                return probability;
            }

            // 判断满减是否符合折扣标准
            CouponTemplateSDK templateSDK = settlement.CouponAndTemplateInfos[0].Template;
            double _base = (double)templateSDK.Rule.Discount.Base;
            double _quota = (double)templateSDK.Rule.Discount.Quota;

            // 如果不符合标准, 则直接返回商品总价
            if (goodsSum < _base)
            {
                _log.LogDebug("Current Goods Cost Sum < ManJian Coupon Base!");
                settlement.Cost= goodsSum;
                settlement.CouponAndTemplateInfos = new List<CouponAndTemplateInfo>();
                return settlement;
            }

            // 计算使用优惠券之后的价格 - 结算
            settlement.Cost = 
                    (goodsSum - _quota) > MinCost ? (goodsSum - _quota) : MinCost;
            _log.LogDebug($"Use ManJian Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");

            return settlement;
        }

        public RuleFlag GetRuleConfig()
        {
            return RuleFlag.MANJIAN;
        }
    }
}
