using Kasbio.Coupon.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Common.Clients
{
    public interface ITemplateClient
    {
        Task<ResponseMessage<List<CouponTemplateSDK>>> FindAllUsableTemplateAsync();

        Task<ResponseMessage<Dictionary<int, CouponTemplateSDK>>> FindId2TemplateSdkAsync(params int[] ids);
    }
}
