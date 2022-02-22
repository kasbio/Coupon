using Coupon.Common.Extension.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace System.Text.Json
{
    public static class JsonExtension
    {
        private static JsonSerializerOptions option;
        static JsonExtension()
        {
           
            option = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };
            option.Converters.Add(new DateTimeFormatterConverter("yyyy-MM-dd HH:mm:ss"));
            option.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

        }


        public static string ToJson(this object obj)
        {
            var json = JsonSerializer.Serialize(obj, option);
            return json;
        }

    }
}
