using Coupon.Common;
using Coupon.Common.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Settlement.Grpc.Model;

namespace Coupon.Settlement.ApplicationCore.Executor
{
    public class RuleExecutor
    {
        private static Dictionary<CouponCategory, IRuleExecutor> executorIndex =
    new Dictionary<CouponCategory, IRuleExecutor>();

        private ILogger<RuleExecutor> logger;

        public RuleExecutor(ILogger<RuleExecutor> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 收集处理器
        /// </summary>
        /// <param name="app"></param>
        public void Collect(IServiceProvider app)
        {
            if (executorIndex.Any())
            {
                return;
            }
            var list = app.GetServices<IRuleExecutor>();
            foreach (var item in list)
            {
                var config = item.GetRuleConfig();
                if (!executorIndex.ContainsKey(config))
                {
                    executorIndex.Add(config, item);
                }
            }

        }


        public SettlementInfos ComputeRule(SettlementInfos settlement)
        {
            SettlementInfos result = null;
            if (settlement.CouponInfos is null)
            {
                logger.LogDebug($"结算实体{nameof(settlement)} 无优惠券");
                return null;
            }

            var category =(CouponCategory) settlement.CouponInfos[0]
                     .Template.Category;

            result = executorIndex[category].ComputeRule(settlement);
     

            return result;
        }


    }
}
