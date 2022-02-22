using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Constant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor.Impl
{
    public class LiJianExecutor : BaseExecutor, IRuleExecutor
    {


        private readonly ILogger<LiJianExecutor> _log;

        public LiJianExecutor(ILogger<LiJianExecutor> log)
        {
            this._log = log;
        }



        public SettlementInfo ComputeRule(SettlementInfo settlement)
        {
            double goodsSum = GoodsCostSum(settlement.GoodsInfos);
                
 
            SettlementInfo probability = ProcessGoodsTypeNotSatisfy(
                    settlement, goodsSum
            );
            if (null != probability)
            {
                _log.LogDebug("LiJian Template Is Not Match To GoodsType!");
                return probability;
            }

            // 立减优惠券直接使用, 没有门槛
            CouponTemplateSDK templateSDK = settlement.CouponAndTemplateInfos[0].Template;
            double quota = (double)templateSDK.Rule.Discount.Quota;

            // 计算使用优惠券之后的价格 - 结算
            settlement.Cost = goodsSum - quota > MinCost ? goodsSum - quota : MinCost;

            _log.LogDebug($"Use LiJian Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");
                    
            return settlement;
        }

        /// <summary>
        /// 规则类型标记
        /// </summary>
        /// <returns></returns>
        public RuleFlag GetRuleConfig()
        {
            return RuleFlag.LIJIAN;
        }
    }
}
