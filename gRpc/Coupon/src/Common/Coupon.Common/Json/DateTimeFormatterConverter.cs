using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coupon.Common.Extension.Json
{
    public class DateTimeFormatterConverter : JsonConverter<DateTime>
    {
        private string formatter = "yyyy-MM-dd HH:mm:ss";
        public DateTimeFormatterConverter(string formatter)
        {
            if (!string.IsNullOrEmpty(formatter))
            {
                this.formatter = formatter;
            }

        }

        public DateTimeFormatterConverter()
        {

        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(formatter));
        }
    }
}
