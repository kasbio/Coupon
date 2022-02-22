using Kasbio.Coupon.Common.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Common.Clients.Impl
{
    public class SettlementClient : ISettlementClient
    {
        private const string CLIENT_NAME = "Settlement";

        private readonly IHttpClientFactory factory;

        private readonly HttpClient client;

        public SettlementClient(IHttpClientFactory factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient(CLIENT_NAME);
        }
        public async Task<ResponseMessage<SettlementInfo>> ComputeRuleAsync(SettlementInfo settlement)
        {
            var res = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "settlement/compute"));
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"response:{res.StatusCode}    message:{res.Content.ToString()}");
            }
            string r = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseMessage<SettlementInfo>>(r);
            
            return result;
        }
    }
}
