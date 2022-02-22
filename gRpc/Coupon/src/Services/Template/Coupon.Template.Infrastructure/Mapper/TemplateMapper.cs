using AutoMapper;
using Coupon.Common.Model;
using Coupon.Template.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coupon.Template.Grpc.Model;

namespace Coupon.Template.Infrastructure.Mapper
{
    internal class TemplateMapper: Profile
    {
        public TemplateMapper()
        {
            CreateMap<TemplateRequest, CouponTemplate>();
        }

    }
}
