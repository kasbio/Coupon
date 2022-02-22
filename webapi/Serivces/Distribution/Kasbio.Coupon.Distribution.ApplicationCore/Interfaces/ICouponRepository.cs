using Kasbio.Coupon.Common.Interfaces;
using Kasbio.Coupon.Distribution.ApplicationCore.Contant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Interfaces
{
    public interface ICouponRepository: IAsyncRepository<Entity.Coupon>
    {
        Task<List<Entity.Coupon>> GetAllByUserIdAndStatusAsync(long userId, CouponStatus status);

    }
}
