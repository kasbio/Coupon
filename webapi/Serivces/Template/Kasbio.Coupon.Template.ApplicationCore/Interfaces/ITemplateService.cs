using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Template.ApplicationCore.Interfaces
{
    public interface ITemplateService
    {
        Task<CouponTemplate> BuildTemplateInfo(int id);

        Task<List<CouponTemplateSDK>> FindAllUsableTemplate();

        Task<Dictionary<int, CouponTemplateSDK>> FindIds2TemplateSDK(List<int> ids);
        /// <summary>
        /// 创建优惠券模板实体
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CouponTemplate> BuildTemplateAsync(TemplateRequest request);

    }
}
