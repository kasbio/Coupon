using Coupon.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Settlement.Grpc.Model;

namespace Coupon.Settlement.ApplicationCore.Executor
{
    internal class BaseExecutor
    {
        /// <summary>
        /// 需要注意:
        /// 1. 这里实现的单品类优惠券的校验, 多品类优惠券重载此方法
        /// 2. 商品只需要有一个优惠券要求的商品类型去匹配就可以
        /// </summary>
        /// <param name="settlement"></param>
        /// <returns></returns>
        protected virtual bool IsGoodsTypeSatisfy(
            SettlementInfos settlement)
        {
            List<int> goodsType = settlement.GoodsInfos
                .Select(o => o.Type).ToList();
            List<int> templateGoodsType = null;
            if (!string.IsNullOrEmpty(settlement.CouponInfos[0]?
                    .Template?.Rule?
                    .Usage?.Goodstype) )
            {
                templateGoodsType =
                JsonConvert.DeserializeObject<List<int>>(
                    settlement.CouponInfos[0]?
                                .Template?.Rule?
                                  .Usage?.Goodstype
                    );
            }



            // 存在交集即可
            return goodsType.Intersect(templateGoodsType).Any();

        }

        /// <summary>
        ///  处理商品类型与优惠券限制不匹配的情况
        /// </summary>
        /// <param name="settlement"></param>
        /// <param name="goodsSum"></param>
        /// <returns></returns>
        protected virtual SettlementInfos ProcessGoodsTypeNotSatisfy(
           SettlementInfos settlement, double goodsSum)
        {
            bool isGoodsTypeSatisfy = IsGoodsTypeSatisfy(settlement);

            // 当商品类型不满足时, 直接返回总价, 并清空优惠券
            if (!isGoodsTypeSatisfy)
            {
                settlement.Cost = goodsSum;
                settlement.CouponInfos.Clear();
                return settlement;
            }

            return null;
        }

        /// <summary>
        /// 商品总价
        /// </summary>
        /// <param name="goodsInfos"></param>
        /// <returns></returns>
        protected virtual double GoodsCostSum(IEnumerable<GoodsInfos> goodsInfos)
        {
            return goodsInfos.Sum(o => o.Price * o.Count);

        }


        /// <summary>
        /// 最小支付费用(不能被白嫖)
        /// </summary>
        protected double MinCost { get { return 0.1; } }
    }
}
