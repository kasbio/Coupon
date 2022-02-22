using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Constant;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor
{
    public class ExecuteManager
    {
        private static Dictionary<RuleFlag, IRuleExecutor> executorIndex =
            new Dictionary<RuleFlag, IRuleExecutor>();

        /// <summary>
        /// 收集处理器
        /// </summary>
        /// <param name="service"></param>
        public static void Collect(IApplicationBuilder app)
        {
            var list = app.ApplicationServices.GetServices<IRuleExecutor>().ToList();
            foreach (var item in list)
            {
                var config = item.GetRuleConfig();
                if (executorIndex.ContainsKey(config))
                {
                    // 跳过，暂不处理
                    continue;
                }
                executorIndex.Add(config, item);

            }
        }

        public SettlementInfo ComputeRule(SettlementInfo settlement)
        {
            SettlementInfo result = null;
            // 单类优惠券
            if (settlement.CouponAndTemplateInfos.Count == 1)
            {

                // 获取优惠券的类别
                CouponCategory category = CouponCategory.Find(
                        settlement.CouponAndTemplateInfos[0]
                                .Template.Category
                );

                switch (category.Code)
                {
                    case "001":

                        result = executorIndex[RuleFlag.MANJIAN]
                                .ComputeRule(settlement);
                        break;
                    case "002":
                        result = executorIndex[RuleFlag.ZHEKOU]
                                .ComputeRule(settlement);
                        break;
                    case "003":
                        result = executorIndex[RuleFlag.LIJIAN]
                                .ComputeRule(settlement);
                        break;
                }
            }
            else
            {

                // 多类优惠券
                List<CouponCategory> categories = new List<CouponCategory>();

                settlement.CouponAndTemplateInfos.ForEach(o =>
                        categories.Add(CouponCategory.Find(
                                o.Template.Category
                        )));
                if (categories.Count != 2)
                {
                    throw new CouponException("Not Support For More " +
                            "Template Category");
                }
                else
                {
                    if (categories.Contains(CouponCategory.MANJIAN)
                            && categories.Contains(CouponCategory.ZHEKOU))
                    {
                        result = executorIndex[RuleFlag.MANJIAN_ZHEKOU]
                                .ComputeRule(settlement);
                    }
                    else
                    {
                        throw new CouponException("Not Support For Other " +
                                "Template Category");
                    }
                }
            }

            return result;
        }

    }
}
