using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    /// <summary>
    /// 使用范围条件
    /// </summary>
    public class Usage
    {

        public Usage(string province,string city,string goodsType)
        {
            this.Province = province;
            this.City = goodsType;
            this.GoodsType = goodsType;
        }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 商品类型，list[文娱，生鲜，家居，全品类]
        /// </summary>
        public string GoodsType { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(Province)
                    && !string.IsNullOrEmpty(City)
                    && !string.IsNullOrEmpty(GoodsType);
        }
        public Usage()
        {

        }
    }
}
