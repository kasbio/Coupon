using Coupon.Common.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Settlement.Grpc.Model;

namespace Coupon.Settlement.ApplicationCore.Executor.Impl
{
    internal class FullCutExecutor : BaseExecutor, IRuleExecutor
    {
        private readonly ILogger<FullCutExecutor> _log;

        public FullCutExecutor(ILogger<FullCutExecutor> log)
        {
            _log = log;
        }

        public SettlementInfos ComputeRule(SettlementInfos settlement)
        {
            double goodsSum =
                GoodsCostSum(settlement.GoodsInfos);
            SettlementInfos probability = ProcessGoodsTypeNotSatisfy(
                    settlement, goodsSum
            );
            if (probability != null)
            {
                _log.LogDebug("ManJian Template Is Not Match To GoodsType!");
                return probability;
            }

            // 判断满减是否符合折扣标准
            var templateSDK = settlement.CouponInfos[0].Template;
            double _base = (double)templateSDK.Rule.Discount.Base;
            double _quota = (double)templateSDK.Rule.Discount.Quota;

            // 如果不符合标准, 则直接返回商品总价
            if (goodsSum < _base)
            {
                _log.LogDebug("Current Goods Cost Sum < ManJian Coupon Base!");
                settlement.Cost = goodsSum;
                settlement.CouponInfos.Clear();
                return settlement;
            }

            // 计算使用优惠券之后的价格 - 结算
            settlement.Cost =
                    (goodsSum - _quota) > MinCost ? (goodsSum - _quota) : MinCost;
            _log.LogDebug($"Use ManJian Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");
            settlement.Employ = true;
            return settlement;
        }

        public CouponCategory GetRuleConfig()
        {
            return CouponCategory.FullCut;
        }
    }
}
