using Kasbio.Coupon.Common.Converter;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Kasbio.Coupon.Template.ApplicationCore.Converters
{
    public class TemplateJsonConverter : JsonConverter<CouponTemplate>
    {

        public override CouponTemplate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<CouponTemplate>(reader.GetString(),options);
        }

        public override void Write(Utf8JsonWriter writer, CouponTemplate value, JsonSerializerOptions options)
        {
   
            
            writer.WriteStartObject();

            writer.WriteString("id", value.Id.ToString());
            writer.WriteString("name", value.Name.ToString());
            writer.WriteString("logo", value.Logo.ToString());
            writer.WriteString("desc", value.Desc??string.Empty);
            writer.WriteString("category", value.Category.Description.ToString());
            writer.WriteString("productline", value.ProductLine.Descrtiption.ToString());
            writer.WriteString("count", value.Count.ToString());
            writer.WriteString("createTime", value.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            writer.WriteString("userId", value.UserId.ToString());
            writer.WriteString("key", value.Key.ToString() + value.Id.ToString("D4") );
            writer.WriteString("target", value.Target.Descrtiption.ToString());
            var rule =  JsonEncodedText.Encode(TemplateRule.Serialize(value.Rule), JavaScriptEncoder.UnsafeRelaxedJsonEscaping);

            writer.WriteString("rule", rule);
           
            writer.WriteEndObject();
        }
    }
}
