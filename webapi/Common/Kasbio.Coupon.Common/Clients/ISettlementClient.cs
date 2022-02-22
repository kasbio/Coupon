using Kasbio.Coupon.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Common.Clients
{
    public interface ISettlementClient
    {

        Task<ResponseMessage<SettlementInfo>> ComputeRuleAsync(SettlementInfo settlement);

    }
}
