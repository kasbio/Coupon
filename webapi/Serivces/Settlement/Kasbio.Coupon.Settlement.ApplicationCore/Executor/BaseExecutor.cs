using Kasbio.Coupon.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Kasbio.Coupon.Settlement.ApplicationCore.Executor
{
    /// <summary>

    /// </summary>
    public class BaseExecutor
    {
        /// <summary>
        /// 需要注意:
        /// 1. 这里实现的单品类优惠券的校验, 多品类优惠券重载此方法
        /// 2. 商品只需要有一个优惠券要求的商品类型去匹配就可以
        /// </summary>
        /// <param name="settlement"></param>
        /// <returns></returns>
        protected virtual bool IsGoodsTypeSatisfy(
            SettlementInfo settlement)
        {
            List<int> goodsType = settlement.GoodsInfos
                .Select(o => o.Type).ToList();

            List<int> templateGoodsType =
                JsonConvert.DeserializeObject<List<int>>(
                    settlement.CouponAndTemplateInfos[0]
                                .Template.Rule.Usage.GoodsType);

            // 存在交集即可
            return goodsType.Intersect(templateGoodsType).Any();

        }

        /// <summary>
        ///  处理商品类型与优惠券限制不匹配的情况
        /// </summary>
        /// <param name="settlement"></param>
        /// <param name="goodsSum"></param>
        /// <returns></returns>
        protected virtual SettlementInfo ProcessGoodsTypeNotSatisfy(
           SettlementInfo settlement, double goodsSum)
        {
            bool isGoodsTypeSatisfy = IsGoodsTypeSatisfy(settlement);

            // 当商品类型不满足时, 直接返回总价, 并清空优惠券
            if (!isGoodsTypeSatisfy)
            {
                settlement.Cost = goodsSum;
                settlement.CouponAndTemplateInfos = new List<CouponAndTemplateInfo>();
                return settlement;
            }

            return null;
        }

        /// <summary>
        /// 商品总价
        /// </summary>
        /// <param name="goodsInfos"></param>
        /// <returns></returns>
        protected virtual double GoodsCostSum(List<GoodsInfo> goodsInfos)
        {
            return goodsInfos.Sum(o => o.Price * o.Count);

        }


        /// <summary>
        /// 最小支付费用
        /// </summary>
        protected double MinCost { get { return 0.1; } }



    }
}
