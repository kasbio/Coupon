using Coupon.Template.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coupon.Template.DbContexts;

namespace Coupon.Template.Infrastructure
{
    public static  class TemplateRepository
    {


        public static async Task<List<CouponTemplate>> GetByNameAsync(this TemplateDbContext dbContext,string name)
        {
            return await dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => o.Name == name)
                .ToListAsync();
        }

        public static async Task<List<CouponTemplate>> GetAllByAvailableAndExpiredAsync(this TemplateDbContext dbContext, bool available, bool expired)
        {
            return await dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => o.Available == available && o.Expired == expired)
                .ToListAsync();
        }

        public static async Task<List<CouponTemplate>> GetAllByIdsAsync(this TemplateDbContext dbContext, List<int> ids)
        {
            return await dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }

        public static async Task<List<CouponTemplate>> GetAllByExpiedAsync(this TemplateDbContext dbContext, bool isExpired)
        {
            return await dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => o.Expired == isExpired)
                .ToListAsync();
        }
    }
}
