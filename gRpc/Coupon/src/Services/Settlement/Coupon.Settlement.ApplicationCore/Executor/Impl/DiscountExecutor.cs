using Coupon.Common.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Settlement.Grpc.Model;
using Coupon.Template.Grpc.Model;

namespace Coupon.Settlement.ApplicationCore.Executor.Impl
{
    internal class DiscountExecutor : BaseExecutor, IRuleExecutor
    {
        private readonly ILogger<DiscountExecutor> _log;

        public DiscountExecutor(ILogger<DiscountExecutor> log)
        {
            _log = log;
        }

        public SettlementInfos ComputeRule(SettlementInfos settlement)
        {
            double goodsSum = GoodsCostSum(settlement.GoodsInfos);
            SettlementInfos probability = ProcessGoodsTypeNotSatisfy(settlement, goodsSum);
            if (probability != null)
            {
                _log.LogDebug("ZheKou Template Is Not Match GoodsType!");
                return probability;
            }

            // 折扣优惠券可以直接使用, 没有门槛
            CouponTemplateDTO templateSDK = settlement.CouponInfos[0].Template;
            double quota = (double)templateSDK.Rule.Discount.Quota;

            // 计算使用优惠券之后的价格
            settlement.Cost =
              (goodsSum * (quota * 1.0 / 100)) > MinCost ? (goodsSum * (quota * 1.0 / 100)) : MinCost
            ;
            _log.LogDebug($"Use ZheKou Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");
            settlement.Employ = true;
            return settlement;
        }

        public CouponCategory GetRuleConfig()
        {
            return CouponCategory.Discount;
        }
    }
}
