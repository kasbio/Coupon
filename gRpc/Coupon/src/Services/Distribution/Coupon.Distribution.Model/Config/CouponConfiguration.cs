using Coupon.Distribution.Model.Config.Convert;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coupon.Distribution.Model.Config;

public class CouponConfiguration: IEntityTypeConfiguration<Model.Coupon>
{
    public void Configure(EntityTypeBuilder<Model.Coupon> builder)
    {
        

        builder.Property(o => o.Template).HasConversion(CouponTemplateDTOConverter.GetConverter()).IsRequired(false);
    }
}