using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Dto;
using MediatR;

namespace Kasbio.Coupon.Template.ApplicationCore.Command
{
    /// <summary>
    /// 通过该命令在数据库中创建<see cref="CouponTemplate"/> 
    /// </summary>
    public class CreateCouponTemplateFromTemplateRequestCommand:IRequest<CouponTemplate>
    {
        private TemplateRequest _template;

        public CreateCouponTemplateFromTemplateRequestCommand(TemplateRequest template)
        {
            this._template = template;
        }

        public TemplateRequest Get()
        {
            return _template;
        }
    }
}
