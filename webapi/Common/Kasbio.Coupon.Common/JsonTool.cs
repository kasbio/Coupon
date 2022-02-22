using Kasbio.Coupon.Common.Converter;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Kasbio.Coupon.Common
{
   public static class JsonTool
    {
        private static JsonSerializerOptions option;
        static JsonTool()
        {
              option = new JsonSerializerOptions()
            {
                IgnoreNullValues = false
            };
            option.Converters.Add(new DateTimeFormatterConverter("yyyy-MM-dd HH:mm:ss"));
            option.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public static string Serialize(object o)
        {
            return JsonSerializer.Serialize(o, option);
        }
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
