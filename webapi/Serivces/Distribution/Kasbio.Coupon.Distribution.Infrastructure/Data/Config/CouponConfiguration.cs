using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using Kasbio.Coupon.Distribution.Infrastructure.Data.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CouponEntity = Kasbio.Coupon.Distribution.ApplicationCore.Entity.Coupon;

namespace Kasbio.Coupon.Distribution.Infrastructure.Data.Config
{
    public class CouponConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.Ignore(o => o.TemplateSDK);
            builder.Property(o => o.Status).HasConversion(CouponStatusConverter.Converter);
        }
    }
}
