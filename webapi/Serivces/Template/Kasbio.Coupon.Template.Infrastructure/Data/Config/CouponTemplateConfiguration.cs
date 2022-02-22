using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Template.Infrastructure.Data.Config
{
   public class CouponTemplateConfiguration : IEntityTypeConfiguration<CouponTemplate>
    {
        public void Configure(EntityTypeBuilder<CouponTemplate> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Ignore(o => o.Desc);
            builder.Property(o => o.Id).IsRequired().UseMySqlIdentityColumn();

            builder.Property(o => o.Category)
                    .HasConversion(CouponCategoryConverter.GetConverter());

            builder.Property(o => o.ProductLine)
                    .HasConversion(ProductLineConverter.GetConverter());

            builder.Property(o => o.Rule)
                    .HasConversion(TemplateRuleConverter.GetConverter());

            builder.Property(o => o.Target)
                    .HasConversion(DistributeTargetConverter.GetConverter());
            builder.Property(o => o.CreateTime).HasDefaultValue();

        }
    }
}
