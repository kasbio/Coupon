using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Dto;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using Kasbio.Coupon.Template.ApplicationCore.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Template.ApplicationCore.Command.Handler
{
    /// <summary>
    /// 传入CouponTemplate创建CouponTemplate
    /// </summary>
    public class CreateCouponTemplateHandler : IRequestHandler<CreateCouponTemplateFromTemplateRequestCommand, CouponTemplate>
    {

        private ITemplateRepository templateRepository;
        public CreateCouponTemplateHandler(ITemplateRepository templateRepository)
        {
            this.templateRepository = templateRepository;
        }

        public async Task<CouponTemplate> Handle(CreateCouponTemplateFromTemplateRequestCommand request, CancellationToken cancellationToken)
        {

            var tRequest = request.Get();
            // 参数合法性校验
            if (!tRequest.Validate())
            {
                throw new CouponException("BuildTemplate Param Is Not Valid!");
            }

            List<CouponTemplate> temp = await templateRepository.GetByNameAsync(tRequest.Name);
            // 判断同名的优惠券模板是否存在
            if (temp == null || temp.Count > 1)
            {
                throw new CouponException("Exist Same Name Template!");
            }

            // 构造 CouponTemplate 并保存到数据库中
            CouponTemplate template = TemplateUtility.RequestToTemplate(tRequest);
            template.CreateTime = DateTime.Now;
            template = await templateRepository.AddAsync(template);
            return template;
        }
    }
}
