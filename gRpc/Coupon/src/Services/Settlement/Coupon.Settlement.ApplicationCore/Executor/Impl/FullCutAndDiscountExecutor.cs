using Coupon.Common.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Settlement.Grpc.Model;

namespace Coupon.Settlement.ApplicationCore.Executor.Impl
{
    internal class FullCutAndDiscountExecutor : BaseExecutor, IRuleExecutor
    {
        private readonly ILogger<FullCutAndDiscountExecutor> _log;

        public FullCutAndDiscountExecutor(ILogger<FullCutAndDiscountExecutor> log)
        {
            _log = log;
        }

        public SettlementInfos ComputeRule(SettlementInfos settlement)
        {
            double goodsSum = GoodsCostSum(settlement.GoodsInfos);
            // 商品类型的校验
            SettlementInfos probability = ProcessGoodsTypeNotSatisfy(
                    settlement, goodsSum
            );
            if (probability != null)
            {
                _log.LogDebug("ManJian And ZheKou Template Is Not Match To GoodsType!");
                return probability;
            }

            CouponWithTemplateDTO manJian = null;
            CouponWithTemplateDTO zheKou = null;

            foreach (var ct in settlement.CouponInfos)
            {
                if (ct.Template.Category ==
                        (int)CouponCategory.FullCut)
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
                settlement.CouponInfos.Clear();
                return settlement;
            }

            List<CouponWithTemplateDTO> ctInfos = new List<CouponWithTemplateDTO>();
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
            settlement.CouponInfos.Clear();
            settlement.CouponInfos.AddRange(ctInfos);
            settlement.Cost = targetSum > MinCost ? targetSum : MinCost;

            _log.LogDebug($"Use ManJian And ZheKou Coupon Make Goods Cost From {goodsSum} To {settlement.Cost}");
            settlement.Employ = true;
            return settlement;
        }

        /// <summary>
        /// 当前两个优惠券是否可以共用
        /// </summary>
        /// <param name="manJian"></param>
        /// <param name="zheKou"></param>
        /// <returns></returns>
        private bool IsTemplateCanShared(CouponWithTemplateDTO manJian, CouponWithTemplateDTO zheKou)
        {

            string manjianKey = manJian.Template.Key
                    + manJian.Template.Id.ToString("D4");
            string zhekouKey = zheKou.Template.Key
                    + zheKou.Template.Id.ToString("D4");

            List<string> allSharedKeysForManjian = new List<string>();
            //allSharedKeysForManjian.Add(manjianKey);
            if (!string.IsNullOrEmpty(manJian.Template.Rule.Weight))
            {

#pragma warning disable CS8604 // 引用类型参数可能为 null。
                allSharedKeysForManjian.AddRange(JsonConvert.DeserializeObject<List<string>>(
                       manJian.Template.Rule.Weight));
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }


            List<string> allSharedKeysForZhekou = new List<string>();
            //allSharedKeysForZhekou.Add(zhekouKey);
            if (!string.IsNullOrEmpty(zheKou.Template.Rule.Weight))
            {
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                allSharedKeysForZhekou.AddRange(JsonConvert.DeserializeObject<List<string>>(
        zheKou.Template.Rule.Weight));
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }


            //List<string> union = new List<string>() { manjianKey, zhekouKey };

            //判断是否为子集
            return !allSharedKeysForManjian/*.Except(union)*/.Any() || !allSharedKeysForZhekou/*.Except(union)*/.Any();


        }


        public CouponCategory GetRuleConfig()
        {
            return CouponCategory.FullCut | CouponCategory.Discount;
        }
    }
}
