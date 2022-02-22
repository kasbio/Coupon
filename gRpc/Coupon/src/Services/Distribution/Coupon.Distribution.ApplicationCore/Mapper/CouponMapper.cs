using Coupon.Common.Model;
using Coupon.Distribution.Grpc.Model;
using Coupon.Template.Grpc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace Coupon.Distribution.ApplicationCore.Mapper
{
    public class CouponMapper:AutoMapper.Profile
    {
        public CouponMapper()
        {

            CreateMap<Model.Coupon, CouponDTO>()
                .ForMember(o => o.AssignTime,opt=> opt.MapFrom(o => o.AssignTime.ToUniversalTime().ToTimestamp()));

            CreateMap<CouponDTO, Model.Coupon>()
                .ForMember(o => o.AssignTime, opt => opt.MapFrom(o => o.AssignTime.ToDateTime()));


        }

    }
}
