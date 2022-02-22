using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Distribution.ApplicationCore.DTO;
using Kasbio.Coupon.Distribution.ApplicationCore.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Distribution.Web.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserServiceController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserServiceController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("coupons")]
        public async Task<List<ApplicationCore.Entity.Coupon>> FindCouponsByStatusAsync(long userId, int status)
        {
            return await _userService.FindCouponsByStatusAsync(userId, status);

        }

        [HttpGet("template")]
        public async Task<List<CouponTemplateSDK>> FindAvailableTemplateAsync(long userId)
        {
            return await _userService.FindAvailableTemplateAsync(userId);
        }
        [HttpPost("/acquire/template")]
        public async Task<ApplicationCore.Entity.Coupon> AcquireTemplateAsync(AcquireTemplateRequest request)
        {
            return await _userService.AcquireTemplateAsync(request);
        }

        [HttpPost("/settlement")]
        public async Task<SettlementInfo> SettlementAsync(SettlementInfo info)
        {
            return await _userService.SettlementAsync(info);
        }
    }
}
