using Kasbio.Coupon.Common.Interfaces;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Template.ApplicationCore.Interfaces
{
    public interface ITemplateRepository : IAsyncRepository<CouponTemplate>
    {

        Task<List<CouponTemplate>> GetByNameAsync(string name);

        Task<List<CouponTemplate>> GetAllByAvailableAndExpired(bool available, bool expired);

        Task<List<CouponTemplate>> GetAllByIds(List<int> ids);

        Task<List<CouponTemplate>> GetAllByExpied(bool isExpired);
    }
}
