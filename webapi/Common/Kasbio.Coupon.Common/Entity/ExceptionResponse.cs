using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.Entity
{
   public class ExceptionResponse
    {
        public ExceptionResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }

    }
}
