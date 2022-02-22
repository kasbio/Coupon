using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CouponEntity =  Kasbio.Coupon.Distribution.ApplicationCore.Entity.Coupon;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Converter.JSON
{
    public class CouponJsonConverter : JsonConverter<CouponEntity>
    {
        public override CouponEntity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
         
            return JsonSerializer.Deserialize<CouponEntity>(reader.GetString(), options);
        }

        public override void Write(Utf8JsonWriter writer, CouponEntity value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id.ToString());
            writer.WriteString("templateId",value.TemplateId.ToString());
            writer.WriteString("userId",value.UserId.ToString());
            writer.WriteString("couponCode",value.CouponCode);
            writer.WriteString("assignTime",value.AssignTime.ToString("yyyy-MM-dd HH:mm:ss"));
            writer.WriteString("name",value.TemplateSDK.Name);
            writer.WriteString("logo",value.TemplateSDK.Logo);
            writer.WriteString("desc",value.TemplateSDK.Desc);
            writer.WriteString("expiration",JsonSerializer.Serialize(value.TemplateSDK.Rule.Expiration,options));
            writer.WriteString("discount", JsonSerializer.Serialize(value.TemplateSDK.Rule.Discount, options));
            writer.WriteString("usage", JsonSerializer.Serialize(value.TemplateSDK.Rule.Usage, options));
            writer.WriteEndObject();


        }
    }
}
