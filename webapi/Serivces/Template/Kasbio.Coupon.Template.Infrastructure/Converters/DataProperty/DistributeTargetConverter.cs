using JetBrains.Annotations;
using Kasbio.Coupon.Common.Constant;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Kasbio.Coupon.Template.Infrastructure.Converters
{
    public class DistributeTargetConverter : ValueConverter<DistributeTarget, int> 
    {
        DistributeTargetConverter([NotNull] Expression<Func<DistributeTarget, int>> convertToProviderExpression, [NotNull] Expression<Func<int, DistributeTarget>> convertFromProviderExpression, [CanBeNull] ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

       public static DistributeTargetConverter GetConverter()
        {
            return new DistributeTargetConverter
                (o => o.Code, o => DistributeTarget.Find(o));
        }
    }
}
