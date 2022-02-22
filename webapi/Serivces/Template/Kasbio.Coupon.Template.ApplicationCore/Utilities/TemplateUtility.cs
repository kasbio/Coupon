using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Template.ApplicationCore.Utilities
{
   public static class TemplateUtility
    {
        public static CouponTemplate RequestToTemplate(TemplateRequest request)
        {
            return new CouponTemplate(
                  request.Name,
                  request.Logo,
                  request.Desc,
                  request.Category,
                  request.ProductLine,
                  request.Count,
                  request.UserId,
                  request.Target,
                  request.Rule
          );
        }


        public static  CouponTemplateSDK Template2TemplateSDK(CouponTemplate template)
        {

            return new CouponTemplateSDK(
                    template.Id,
                    template.Name,
                    template.Logo,
                    template.Desc,
                    template.Category.Code,
                    template.ProductLine.Code,
                    template.Key,  // 并不是拼装好的 Template Key
                    template.Target.Code,
                    template.Rule
            );
        }

    }
}
