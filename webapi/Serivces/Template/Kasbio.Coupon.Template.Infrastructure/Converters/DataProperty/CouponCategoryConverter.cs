using JetBrains.Annotations;
using Kasbio.Coupon.Common.Constant;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Kasbio.Coupon.Template.Infrastructure.Converters
{
    public class CouponCategoryConverter : ValueConverter<CouponCategory, string>
    {
        CouponCategoryConverter([NotNull] Expression<Func<CouponCategory, string>> convertToProviderExpression, [NotNull] Expression<Func<string, CouponCategory>> convertFromProviderExpression, [CanBeNull] ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

        public static CouponCategoryConverter GetConverter()
        {
            return new CouponCategoryConverter(
                o => o.Code, o => CouponCategory.Find(o));
        }
    }
}
