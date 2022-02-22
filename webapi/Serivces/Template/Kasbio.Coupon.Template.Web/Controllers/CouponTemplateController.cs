using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using Kasbio.Coupon.Template.ApplicationCore.Entities.Dto;

namespace Kasbio.Coupon.Template.Web.Controllers
{
    [ApiController]
    [Route("template")]
    public class CouponTemplateController:ControllerBase
    {

        private readonly ILogger<CouponTemplateController> _log;
        private readonly ITemplateService _templateService;
        public CouponTemplateController(ILogger<CouponTemplateController> log,ITemplateService templateService)
        {
            _log = log;
            _templateService = templateService;
        }


        [HttpPost("build")]
        public async Task<CouponTemplate> BuildTemplate([FromBody]TemplateRequest request)
        {
            if (request ==  null)
            {
                throw new CouponException("Parameter [Rule] is Empty");
            }

            _log.LogInformation($"[Controller]BuildTemplate: TemplateRequest:{request.ToString()}");

            var result =  await _templateService.BuildTemplateAsync(request);

            return result;



        }

        [HttpGet("info")]

        public async Task<CouponTemplate> BuildTemplateInfo(int id)
        {
            _log.LogDebug($"BuildTemplateInfo: id:{id}");

            var result = await _templateService.BuildTemplateInfo(id);


            return result;
        }


        [HttpGet("sdk/all")]
        public async Task<List<CouponTemplateSDK>> FindAllUsableTemplate()
        {
             
            return await _templateService.FindAllUsableTemplate();
        }


        [HttpGet("sdk/infos")]

        public async Task<IActionResult> FindId2TemplateSdk([FromQuery]int[] ids)
        {
            if (ids != null && ids.Length <= 0)
            {
                throw new CouponException("无效参数");
            }

            var result =  await _templateService.FindIds2TemplateSDK(ids.ToList());
            var reps =  ResponseMessage<Dictionary<int, CouponTemplateSDK>>.GetSuccessResponse(result);
            return new JsonResult(reps);

        }
    }
}
