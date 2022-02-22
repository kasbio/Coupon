using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;

namespace Kasbio.Coupon.Distribution.Infrastructure.Data.Converter
{
    public class CouponStatusConverter : ValueConverter<CouponStatus, int>
    {
         CouponStatusConverter([NotNullAttribute] Expression<Func<CouponStatus, int>> convertToProviderExpression,
                        [NotNullAttribute] Expression<Func<int, CouponStatus>> convertFromProviderExpression)
            :base(convertToProviderExpression, convertFromProviderExpression)
        {

        }

        public static CouponStatusConverter Converter = new CouponStatusConverter(o => o.Code, o => CouponStatus.Find(o));

        public static CouponStatusConverter Get()
        {
            return Converter;
        }
    }
}
