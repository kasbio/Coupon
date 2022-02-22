using AutoMapper;
using Coupon.Common.Model;
using Coupon.Template.Grpc.Model;
using Coupon.Template.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Template.ApplicationCore.Mapper
{
    public class CouponTemplateMapper : Profile
    {
        public CouponTemplateMapper()
        {
            CreateMap<CouponTemplateDTO, CouponTemplate>()
            .ForMember(o => o.Category, opt => opt.MapFrom(d => (CouponCategory)d.Category))
            .ForMember(o => o.ProductLine, opt => opt.MapFrom(d => (ProductLine)d.Productline))
            .ForMember(o => o.Target, opt => opt.MapFrom(d => (DistributeTarget)d.Target))
            .ForMember(o => o.CreateTime, opt => opt.MapFrom(d => d.Createtime.ToDateTime()));

            
            CreateMap<CouponTemplate, CouponTemplateDTO>()
                .ForMember(o => o.Productline, opt => opt.MapFrom(d => (ProductLine)d.ProductLine))
                .ForMember(o => o.Createtime, opt => opt.MapFrom(d =>Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(d.CreateTime.ToUniversalTime()) ));

                 

            CreateMap<TemplateRequest, CouponTemplate>()
                .ReverseMap();
        }

    }
}
