using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Kasbio.Coupon.Template.Infrastructure.Data
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions option) :base(option)
        {

        }
        public DbSet<CouponTemplate>  Templates{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
