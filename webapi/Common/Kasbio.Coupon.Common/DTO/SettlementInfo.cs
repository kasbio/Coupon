using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    public class SettlementInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        public List<CouponAndTemplateInfo> CouponAndTemplateInfos { get; set; }
        /// <summary>
        /// 结算的商品类型
        /// </summary>
        public List<GoodsInfo> GoodsInfos { get; set; }
        /// <summary>
        /// 结果结算金额
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// 是否使结算生效，即核销
        /// </summary>
        public bool Employ { get; set; }


        public SettlementInfo()
        {

        }

        public SettlementInfo(long userId,List<CouponAndTemplateInfo> infos,List<GoodsInfo> goodinfos,double cost,bool employ)
        {
            this.UserId = userId;
            this.CouponAndTemplateInfos = infos;
            this.GoodsInfos = GoodsInfos;
            this.Cost = cost;
            this.Employ = employ;
        }

    }
}
