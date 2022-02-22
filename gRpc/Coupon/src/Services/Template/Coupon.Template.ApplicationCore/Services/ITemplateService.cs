using Coupon.Common.Model;
using Coupon.Template.Grpc.Model;
using Coupon.Template.Model;

namespace Coupon.Template.ApplicationCore.Services
{
    public interface ITemplateService
    {

        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CouponTemplateDTO> GetTempalteAsync(int id);

        /// <summary>
        /// 获取可用模板
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CouponTemplateDTO>> FindAllUsableTemplateAsync();


        Task<CouponTemplateList> GetAllByAvailableAndExpiredAsync(bool available, bool expired);


        /// <summary>
        /// 根据ID获取模板
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<CouponTemplateWithStatus> FindTemplateAsync(IEnumerable<int> ids);
        /// <summary>
        /// 创建优惠券模板实体
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CouponTemplateDTO> BuildTemplateAsync(TemplateRequest request);



    }
}
