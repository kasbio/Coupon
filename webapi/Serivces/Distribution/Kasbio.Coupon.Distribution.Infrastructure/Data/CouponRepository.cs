using Kasbio.Coupon.Common.Data;
using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using Kasbio.Coupon.Distribution.ApplicationCore.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Coupon1 = Kasbio.Coupon.Distribution.ApplicationCore.Entity.Coupon;
using Microsoft.EntityFrameworkCore;

namespace Kasbio.Coupon.Distribution.Infrastructure.Data
{
    public class CouponRepository : EfRepository<Coupon1>, ICouponRepository
    {
        public CouponRepository(CouponDbContext db) : base(db)
        {

        }

        public async Task<List<Coupon1>> GetAllByUserIdAndStatusAsync(long userId, CouponStatus status)
        {
          return await   _dbContext.Set<Coupon1>().AsQueryable()
                .Where(o => o.UserId == userId && o.Status == status)
                .ToListAsync();
        }
    }
}
