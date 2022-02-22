using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    public class GoodsInfo
    {
        /// <summary>
        ///  商品类型 
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Count { get; set; }


        public GoodsInfo(int type,double price,int count)
        {
            this.Type = type;
            this.Price = price;
            this.Count = count;
        }
        public GoodsInfo()
        {

        }
    }
}
