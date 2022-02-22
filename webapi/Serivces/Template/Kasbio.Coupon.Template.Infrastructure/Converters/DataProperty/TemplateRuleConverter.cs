using JetBrains.Annotations;
using Kasbio.Coupon.Common.Converter;
using Kasbio.Coupon.Common.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json;

namespace Kasbio.Coupon.Template.Infrastructure.Converters
{
    public class TemplateRuleConverter : ValueConverter<TemplateRule, string>
    {
        TemplateRuleConverter([NotNull] Expression<Func<TemplateRule, string>> convertToProviderExpression, [NotNull] Expression<Func<string, TemplateRule>> convertFromProviderExpression, [CanBeNull] ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }


        public static TemplateRuleConverter GetConverter()
        {
            return new TemplateRuleConverter(
                o => TemplateRule.Serialize(o), 
                o => TemplateRule.Find(o)
               );
        }
    }
}
