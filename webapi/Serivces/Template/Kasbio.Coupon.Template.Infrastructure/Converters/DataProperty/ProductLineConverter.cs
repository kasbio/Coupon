using JetBrains.Annotations;
using Kasbio.Coupon.Common.Constant;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq.Expressions;

namespace Kasbio.Coupon.Template.Infrastructure.Converters
{
    public class ProductLineConverter : ValueConverter<ProductLine,int>
    {
        ProductLineConverter([NotNullAttribute] Expression<Func<ProductLine, int>> convertToProviderExpression, [NotNullAttribute] Expression<Func<int, ProductLine>> convertFromProviderExpression, [CanBeNullAttribute] ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {

        }

        public static ProductLineConverter GetConverter()
        {
            return   new ProductLineConverter(o => o.Code,
                            o => ProductLine.Find(o));
        }


    }
}
