using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Template.ApplicationCore.Command;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Dto;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using Kasbio.Coupon.Template.ApplicationCore.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Template.ApplicationCore.Services
{
   public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _template;
        private readonly IMediator _mediator;
 
        public TemplateService(ITemplateRepository template, IMediator mediator)
        {
            _template = template;
            _mediator = mediator;
        }

        /// <summary>
        /// 根据TemplateID获取模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="CouponTemplate"/>模板信息</returns>
        public async Task<CouponTemplate> BuildTemplateInfo(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("invaild id");
            }

           var temp = await _template.GetByIdAsync(id);
            if (temp == null)
            {
                throw new CouponException($"coupon id:{id} not exist");
            }

            return temp;
        }

        public async Task<List<CouponTemplateSDK>> FindAllUsableTemplate()
        {
            var templateList =  
               await _template.GetAllByAvailableAndExpired(true, false);

            List<CouponTemplateSDK> temp = new List<CouponTemplateSDK>();

            if (templateList != null && templateList.Count > 0 )
            {
                templateList.ForEach(o => temp.Add(TemplateUtility.Template2TemplateSDK(o)));
            }

            return temp;

        }

        public async Task<Dictionary<int, CouponTemplateSDK>> FindIds2TemplateSDK(List<int> ids)
        {
            var list = await _template.GetAllByIds(ids);

            List<CouponTemplateSDK> temp = new List<CouponTemplateSDK>();
            if (list != null && list.Count >0 )
            {
                list.ForEach(o => temp.Add(TemplateUtility.Template2TemplateSDK(o)));
            }
            var dic =  temp.AsQueryable().GroupBy(o => o.Id).ToDictionary(o => o.Key, o => o.FirstOrDefault());
            return dic;

        }

        public async  Task<CouponTemplate> BuildTemplateAsync(TemplateRequest request)
        {
            //创建优惠券
            CreateCouponTemplateFromTemplateRequestCommand command
                = new CreateCouponTemplateFromTemplateRequestCommand(request);
            CouponTemplate template = await _mediator.Send(command);


            // 根据优惠券模板异步生成优惠券码
            ConstructCouponByTemplateCommand command2 = new ConstructCouponByTemplateCommand(template);
            await _mediator.Send(command2);

            return template;
        }

    }
}
