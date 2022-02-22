
using Coupon.Template.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Coupon.Template.DbContexts
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions option) : base(option)
        {

        }
        public virtual DbSet<CouponTemplate> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
