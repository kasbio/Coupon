using System.Linq.Expressions;
using System.Text.Json;
using Coupon.Template.Grpc.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Coupon.Distribution.Model.Config.Convert;

public class CouponTemplateDTOConverter: ValueConverter<CouponTemplateDTO, string>
{
    public CouponTemplateDTOConverter(Expression<Func<CouponTemplateDTO, string>> convertToProviderExpression, 
        Expression<Func<string, CouponTemplateDTO>> convertFromProviderExpression, 
        ConverterMappingHints? mappingHints = null) 
        : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
        
        
        
    }

    public CouponTemplateDTOConverter(Expression<Func<CouponTemplateDTO, string>> convertToProviderExpression,
        Expression<Func<string, CouponTemplateDTO>> convertFromProviderExpression, 
        bool convertsNulls, ConverterMappingHints? mappingHints = null) 
        : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }

    private static string Serialize(CouponTemplateDTO input)
    {
        return JsonSerializer.Serialize(input);
    }


    private static CouponTemplateDTO Deserialize(string input)
    {
        return JsonSerializer.Deserialize<CouponTemplateDTO>(input) 
               ?? throw new ArgumentNullException(nameof(input));
    }
        
        
    public static CouponTemplateDTOConverter GetConverter()
    {
        return new CouponTemplateDTOConverter(
            convertToProviderExpression: o => Serialize(o),
            convertFromProviderExpression: o => Deserialize(o)
        );
    }
    
}