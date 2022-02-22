using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Data;

namespace Kasbio.Coupon.Template.Infrastructure.Data
{
    public class TemplateRepository : EfRepository<CouponTemplate>, ITemplateRepository
    {
        

        public TemplateRepository(TemplateDbContext dbContext):base(dbContext)
        {
            
        }

        public async Task<List<CouponTemplate>> GetByNameAsync(string name)
        {
            return await _dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => o.Name == name)
                .ToListAsync() ;
        }

        public async Task<List<CouponTemplate>> GetAllByAvailableAndExpired(bool available, bool expired)
        {
            return await _dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => o.Available == available && o.Expired == expired)
                .ToListAsync();
        }

        public async Task<List<CouponTemplate>> GetAllByIds(List<int> ids)
        {
            return await _dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => ids.Contains(o.Id))
                .ToListAsync();
        }

        public async Task<List<CouponTemplate>> GetAllByExpied(bool isExpired)
        {
            return await _dbContext.Set<CouponTemplate>().AsQueryable()
                .Where(o => o.Expired == isExpired)
                .ToListAsync();
        }

    }
}
