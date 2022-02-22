using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Kasbio.Coupon.Common.Converter;
using Kasbio.Coupon.Common.DTO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Kasbio.Coupon.Common.Advice
{
    /// <summary>
    /// Response 自定义json
    /// 格式：{code:'',message:'',data:''}
    /// </summary>
    public class CustomResponseFormatter : TextOutputFormatter
    {

        public CustomResponseFormatter()
        {
            this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            this.SupportedEncodings.Add(Encoding.UTF8);
            this.SupportedEncodings.Add(Encoding.Unicode);
        }



        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            
            return context.ObjectType != typeof(IActionResult);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            JsonSerializerOptions option = new JsonSerializerOptions()
            {
                IgnoreNullValues = false
            };
            option.Converters.Add(new DateTimeFormatterConverter("yyyy-MM-dd HH:mm:ss"));
            option.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            object contextMsg = context.Object is System.Collections.ICollection ? JsonSerializer.Serialize(context.Object,option): context.Object;
          


            var msg = JsonSerializer.Serialize(
                context.ObjectType == typeof(ResponseMessage)?
                context.Object:
                new ResponseMessage<object>(
                    context.HttpContext.Response.StatusCode, 
                    string.Empty,
                    contextMsg),option
            );

            var byteArr = selectedEncoding.GetBytes(msg);
            return  context.HttpContext.Response.Body.WriteAsync(byteArr,0,byteArr.Length);


        }
    }
}
