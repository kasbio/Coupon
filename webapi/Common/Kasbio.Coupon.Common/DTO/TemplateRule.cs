using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Common.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Kasbio.Coupon.Common.DTO
{
    public class TemplateRule
    {

        public TemplateRule(Expiration expiration,Discount discount,int limitation,Usage usage)
        {
            this.Expiration = expiration;
            this.Discount = discount;
            this.Limitation = limitation;
            this.Usage = usage;
        }

        public Expiration Expiration { get; set; }

        public Discount Discount { get; set; }

        public int Limitation { get; set; }

        /// <summary>
        /// 使用范围：地域 + 商品类型
        /// </summary>
        public Usage Usage { get; set; }


        /// <summary>
        ///  权重，可以跟哪些优惠券叠加使用，同一类优惠券一定不能叠加:list[],优惠券唯一编码
        /// </summary>
        public string Weight { get; set; }


        

        public bool Validate()
        {
            return Expiration.Validate() && Discount.Validate()
                    && Limitation > 0 && Usage.Validate()
                    && !string.IsNullOrEmpty(Weight);
        }


        public TemplateRule()
        {

        }


        public static TemplateRule Find(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new CouponException($"Template json is null:{json}");
            }


            return JsonSerializer.Deserialize<TemplateRule>(json );
        }

        public static string Serialize(TemplateRule rule)
        {
            JsonSerializerOptions option = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };
            option.Converters.Add(new DateTimeFormatterConverter("yyyy-MM-dd HH:mm:ss"));
            option.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            var json = JsonSerializer.Serialize(rule, option);
            return json;
        }


        public override string ToString()
        {
            JsonSerializerOptions option = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };
            option.Converters.Add(new DateTimeFormatterConverter("yyyy-MM-dd HH:mm:ss"));
            option.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            var json = JsonSerializer.Serialize(this, option);
            return json;
           
        }

    }




 




}
