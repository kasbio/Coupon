
using Coupon.Template.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Template.Model.Converter;

namespace Coupon.Template.Model.Config
{
    internal class CouponTemplateConfiguration : IEntityTypeConfiguration<CouponTemplate>
    {
        public void Configure(EntityTypeBuilder<CouponTemplate> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired();

 

            builder.Property(o => o.Rule)
                    .HasConversion(TemplateRuleConverter.GetConverter());

 
            //builder.Property(o => o.CreateTime).HasDefaultValue();

        }
    }
}
