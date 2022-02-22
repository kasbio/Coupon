using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Common.Model;
using Coupon.Distribution.DbContexts;
using Coupon.Distribution.Model;
using Coupon.Distribution.Model.Contant;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Distribution.Infrastructure.Repository
{
    public static class CouponRepository
    {
        

        public static async Task<List<Model.Coupon>> GetAllByUserIdAndStatusAsync
            (this CouponDbContext db,long userId, CouponStatus status)
        {
            return await db.Set<Model.Coupon>()
               .Where(o => o.UserId == userId && o.Status == status)
               .ToListAsync();
        }


    }
}
