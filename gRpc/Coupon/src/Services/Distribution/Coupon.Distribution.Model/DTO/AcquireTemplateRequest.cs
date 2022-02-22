using Coupon.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Distribution.Model.DTO
{
    public class AcquireTemplateRequest
    {
        public long UserId { get; set; }

        public int TemplateId { get; set; }
        public AcquireTemplateRequest()
        {

        }

        public AcquireTemplateRequest(long userId, int templateSDK)
        {
            this.UserId = userId;
            this.TemplateId = templateSDK;
        }


    }
}
