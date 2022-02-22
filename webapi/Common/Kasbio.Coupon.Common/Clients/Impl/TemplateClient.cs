using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Common.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kasbio.Coupon.Common.Clients.Impl
{
    public class TemplateClient : ITemplateClient
    {
        private const string CLIENT_NAME = "Template";

        private readonly IHttpClientFactory factory;

        private readonly HttpClient client;

        public TemplateClient(IHttpClientFactory factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient(CLIENT_NAME);
        }

        public async System.Threading.Tasks.Task<ResponseMessage<List<CouponTemplateSDK>>> FindAllUsableTemplateAsync()
        {
        
            var res = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "template/sdk/all"));
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"response:{res.StatusCode}    message:{res.Content.ToString()}");
            }
            string r = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseMessage<List<CouponTemplateSDK>>>(r);
            //JsonTool.Deserialize<ResponseMessage<List<CouponTemplateSDK>>>(r);
            return result;
        }



        public async System.Threading.Tasks.Task<ResponseMessage<Dictionary<int, CouponTemplateSDK>>> FindId2TemplateSdkAsync(params int[] ids)
        {
           
            string query = string.Join("&ids=", ids);

            //var content = new StringContent(JsonConvert.SerializeObject(ids));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"template/sdk/infos?{(string.IsNullOrEmpty(query)?string.Empty:$"ids={query}")}");

            var res = await client.SendAsync(requestMessage);
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"response:{res.StatusCode}    message:{res.Content.ToString()}");
            }
            string r = await res.Content.ReadAsStringAsync();
            var t = JsonConvert.DeserializeObject<SimpleMessage>(r);
            if (t.Code == 200)
            {
                var result = JsonConvert.DeserializeObject<ResponseMessage<Dictionary<int, CouponTemplateSDK>>>(r);
                return result;
            }
            else
            {
                var a = JsonConvert.DeserializeObject <ResponseMessage<ResponseMessage<ExceptionResponse>>> (r);
                throw new Exception(a.Data.Message);
            }
            // return ResponseMessage<Dictionary<int, CouponTemplateSDK>>.GetFailResponse(new Dictionary<int, CouponTemplateSDK>());
            //= JsonTool.Deserialize<ResponseMessage<Dictionary<int, CouponTemplateSDK>>>(r);

        }
    }
}
