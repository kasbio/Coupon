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
    internal class KnockExecutor : BaseExecutor, IRuleExecutor
    {
        private readonly ILogger<KnockExecutor> _logger;

        public KnockExecutor(ILogger<KnockExecutor> logger)
        {
            _logger = logger;
        }

        public SettlementInfos ComputeRule(SettlementInfos settlement)
        {
            double goodsSum = GoodsCostSum(settlement.GoodsInfos);


            SettlementInfos probability = ProcessGoodsTypeNotSatisfy(
                    settlement, goodsSum
            );
            if (null != probability)
            {
                _logger.LogDebug("LiJian Template Is Not Match To GoodsType!");
                return probability;
            }

            // 立减优惠券直接使用, 没有门槛
            var templateSDK = settlement.CouponInfos[0].Template;
            double quota = (double)templateSDK.Rule.Discount.Quota;

            // 计算使用优惠券之后的价格 - 结算
            settlement.Cost = goodsSum - quota > MinCost ? goodsSum - quota : MinCost;

            _logger.LogDebug($"Use LiJian Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");
            settlement.Employ = true;
            return settlement;
        }

        public CouponCategory GetRuleConfig()
        {
            return CouponCategory.Knock;
        }
    }
}
