using AutoMapper;
using Coupon.Distribution.ApplicationCore.Services;
using Coupon.Distribution.Grpc.Model;
using Coupon.Distribution.Model.DTO;
using Coupon.Settlement.Grpc.Model;
using Coupon.Template.Grpc.Model;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Coupon.Distribution.Grpc.Services
{
    public class UserController:UserServices.UserServicesBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task<CouponDTO> Acquire(AcquireRequest request, ServerCallContext context)
        {
            
            return await  _userService.AcquireAsync(request);
        }

        public override async Task<CouponTemplateList> FindAvailableTemplate(UserIdRequest request, ServerCallContext context)
        {
            return await _userService.FindAvailableTemplateAsync(request);
        }

        public override async Task<CouponCollection> FindCouponsByStatus(FindCouponsByStatusRequest request, ServerCallContext context)
        {
            return await _userService.FindCouponsByStatusAsync(request);
        }

        public override async Task<SettlementInfos> Settle(SettlementInfos request, ServerCallContext context)
        {
            return await _userService.SettlementAsync(request);
        }
    }
}
