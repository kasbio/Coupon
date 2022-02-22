using Kasbio.Coupon.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.Extensions
{
  public static  class DateTimeExtension
    {
        public static long ToTimeStamp(this DateTime date)
        {
            return DateTimeUtility.ConvertToTimeStamp(date);
        }
    }
}
