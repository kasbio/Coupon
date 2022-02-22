using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Coupon.Template.Grpc.Model;

namespace Coupon.Template.Model.Converter
{
    internal class TemplateRuleConverter : ValueConverter<TemplateRuleDTO, string>
    {
        TemplateRuleConverter([NotNull] Expression<Func<TemplateRuleDTO, string>> convertToProviderExpression, 
            [NotNull] Expression<Func<string, TemplateRuleDTO>> convertFromProviderExpression, 
            ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

        private static string Serialize(TemplateRuleDTO input)
        {
            
            return JsonHelper.Serialize(input);
        }


        private static TemplateRuleDTO Deserialize(string input)
        {
           return JsonHelper.Deserialize<TemplateRuleDTO>(input) 
                  ?? throw new ArgumentNullException(nameof(input));
        }
        
        
        public static TemplateRuleConverter GetConverter()
        {
            return new TemplateRuleConverter(
                convertToProviderExpression: o => Serialize(o),
                convertFromProviderExpression: o => Deserialize(o)
               );
        }
    }
}
