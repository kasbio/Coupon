using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    public class FallbackResponseMessage : ResponseMessage<string>
    {
        public FallbackResponseMessage(int code, string message, string data) : base(code, message, data)
        {
        }


    }
}
