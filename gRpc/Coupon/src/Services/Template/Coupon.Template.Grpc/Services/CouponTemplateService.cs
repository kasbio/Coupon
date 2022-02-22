
using  Coupon.Template.ApplicationCore.Services;
using Coupon.Template.Grpc.Model;
using Grpc.Core;
using Coupon.Common;
using AutoMapper;
using Coupon.Template.Model;

namespace Coupon.Template.Grpc.Services
{
    public class CouponTemplateService : CouponTemplateServices.CouponTemplateServicesBase
    {
        private readonly ITemplateService _templateService;
        private readonly ILogger<CouponTemplateService> _logger;
 

        public CouponTemplateService(ITemplateService templateService,
         ILogger<CouponTemplateService> logger )
        {
            _templateService = templateService;
            _logger = logger;
          
        }

        public override async Task<CouponTemplateDTO> Build(TemplateRequest request, ServerCallContext context)
        {
            if (request is null)
            {
                throw new CouponException("Parameter [REQUEST] is Empty");
            }
            _logger.LogInformation($"[Controller]BuildTemplate: TemplateRequest:{request.ToString()}");
           
            var result =  await _templateService.BuildTemplateAsync(request);
             
            return result;
        }

        public override async Task<CouponTemplateWithStatus> FindId2TemplateSDK(CouponTemplateIds request, ServerCallContext context)
        {
            if (request is null || request.Ids is null 
                || request.Ids.Count == 0)
            {
                throw new CouponException("无效参数");
            }

            var result = await _templateService.FindTemplateAsync(request.Ids);
            return result;
        }

        public override async Task<CouponTemplateList> GetAllByAvailableAndExpired(CouponTemplateStatus request, ServerCallContext context)
        {
            if (request is null)
            {
                throw new CouponException("无效参数");
            }
            var list = await  _templateService.GetAllByAvailableAndExpiredAsync(request.Available, request.Expired);
            return list;
        }

        public override async Task<CouponTemplateDTO> Infos(CouponTemplateId request, ServerCallContext context)
        {
            if (request is null)
            {
                throw new CouponException("无效参数");
            }
            var r = await _templateService.GetTempalteAsync(request.Id);
            return r;
        }



    }
}
