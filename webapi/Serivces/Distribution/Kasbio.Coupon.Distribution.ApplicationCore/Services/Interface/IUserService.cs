using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Distribution.ApplicationCore.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Distribution.ApplicationCore.Services.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// 根据用户 id 和状态查询优惠券记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<Entity.Coupon>> FindCouponsByStatusAsync(long userId, int status) ;

        /// <summary>
        /// 根据用户 id 查找当前可以领取的优惠券模板
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CouponTemplateSDK>> FindAvailableTemplateAsync(long userId);


        /// <summary>
        /// 用户领取优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Entity.Coupon> AcquireTemplateAsync(AcquireTemplateRequest request);
           


        /// <summary>
        /// 结算(核销)优惠券
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<SettlementInfo> SettlementAsync(SettlementInfo info) ;

    }
}
