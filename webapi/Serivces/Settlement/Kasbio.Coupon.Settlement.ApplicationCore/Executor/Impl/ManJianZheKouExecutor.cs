using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Constant;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor.Impl
{
    public class ManJianZheKouExecutor : BaseExecutor, IRuleExecutor
    {
        private readonly ILogger<ManJianZheKouExecutor> _log;

        public ManJianZheKouExecutor(ILogger<ManJianZheKouExecutor> log)
        {
            this._log = log;
        }


        /// <summary>
        /// 优惠券规则的计算
        /// </summary>
        /// <param name="settlement"></param>
        /// <returns></returns>
        public SettlementInfo ComputeRule(SettlementInfo settlement)
        {
            double goodsSum = GoodsCostSum(settlement.GoodsInfos);
            // 商品类型的校验
            SettlementInfo probability = ProcessGoodsTypeNotSatisfy(
                    settlement, goodsSum
            );
            if (probability != null)
            {
                _log.LogDebug("ManJian And ZheKou Template Is Not Match To GoodsType!");
                return probability;
            }

            CouponAndTemplateInfo manJian = null;
            CouponAndTemplateInfo zheKou = null;

            foreach (CouponAndTemplateInfo ct in settlement.CouponAndTemplateInfos)
            {
                if (CouponCategory.Find(ct.Template.Category) ==
                        CouponCategory.MANJIAN)
                {
                    manJian = ct;
                }
                else
                {
                    zheKou = ct;
                }
            }

            if (manJian == null || zheKou == null)
            {
                throw new Exception("invaild category");
            }

            // 当前的折扣优惠券和满减券如果不能共用(一起使用), 清空优惠券, 返回商品原价
            if (!IsTemplateCanShared(manJian, zheKou))
            {
                _log.LogDebug("Current ManJian And ZheKou Can Not Shared!");
                settlement.Cost = goodsSum;
                settlement.CouponAndTemplateInfos = new List<CouponAndTemplateInfo>();
                return settlement;
            }

            List<CouponAndTemplateInfo> ctInfos = new List<CouponAndTemplateInfo>();
            double manJianBase = (double)manJian.Template.Rule.Discount.Base;

            double manJianQuota = (double)manJian.Template.Rule.Discount.Quota;

            // 最终的价格
            double targetSum = goodsSum;

            // 先计算满减
            if (targetSum >= manJianBase)
            {
                targetSum -= manJianQuota;
                ctInfos.Add(manJian);
            }

            // 再计算折扣
            double zheKouQuota = (double)zheKou.Template.Rule.Discount.Quota;
            targetSum *= zheKouQuota * 1.0 / 100;
            ctInfos.Add(zheKou);

            settlement.CouponAndTemplateInfos = ctInfos;
            settlement.Cost = targetSum > MinCost ? targetSum : MinCost;

            _log.LogDebug($"Use ManJian And ZheKou Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");

            return settlement;
        }

        /// <summary>
        /// 当前两个优惠券是否可以共用
        /// </summary>
        /// <param name="manJian"></param>
        /// <param name="zheKou"></param>
        /// <returns></returns>
        private bool IsTemplateCanShared(CouponAndTemplateInfo manJian, CouponAndTemplateInfo zheKou)
        {

            string manjianKey = manJian.Template.Key
                    + manJian.Template.Id.ToString("D4");
            string zhekouKey = zheKou.Template.Key
                    + zheKou.Template.Id.ToString("D4");

            List<string> allSharedKeysForManjian = new List<string>();
            allSharedKeysForManjian.Add(manjianKey);
            allSharedKeysForManjian.AddRange(JsonConvert.DeserializeObject<List<string>>(
                    manJian.Template.Rule.Weight));

            List<string> allSharedKeysForZhekou = new List<string>();
            allSharedKeysForZhekou.Add(zhekouKey);
            allSharedKeysForZhekou.AddRange(JsonConvert.DeserializeObject<List<string>>(
                    zheKou.Template.Rule.Weight));

            List<string> union = new List<string>() { manjianKey, zhekouKey };

            //判断是否为子集
            return !allSharedKeysForManjian.Except(union).Any() || !allSharedKeysForZhekou.Except(union).Any();


        }

        public RuleFlag GetRuleConfig()
        {
            return RuleFlag.MANJIAN_ZHEKOU;
        }

        protected override bool IsGoodsTypeSatisfy(SettlementInfo settlement)
        {
            _log.LogDebug("Check ManJian And ZheKou Is Match Or Not!");
            List<int> goodsType =
                settlement.GoodsInfos.Select(o => o.Type).ToList();

            List<int> templateGoodsType = new List<int>();

            settlement.CouponAndTemplateInfos.ForEach(o =>
            {

                templateGoodsType.AddRange(JsonConvert.DeserializeObject<int[]>(
                        o.Template.Rule.Usage.GoodsType));

            });

            // 如果想要使用多类优惠券, 则必须要所有的商品类型都包含在内, 即差集为空
            return !goodsType.Except(templateGoodsType).Any();

        }
    }
}
