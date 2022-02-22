using Coupon.Common.Model;
using Coupon.Distribution.Grpc.Model;
using Coupon.Distribution.Model.DTO;
using Coupon.Settlement.Grpc.Model;
using Coupon.Template.Grpc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Distribution.ApplicationCore.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 根据用户 id 和状态查询优惠券记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<CouponCollection> FindCouponsByStatusAsync(FindCouponsByStatusRequest request);

        /// <summary>
        /// 根据用户 id 查找当前可以领取的优惠券模板
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CouponTemplateList> FindAvailableTemplateAsync(UserIdRequest request);


        /// <summary>
        /// 用户领取优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CouponDTO> AcquireAsync(AcquireRequest request);



        /// <summary>
        /// 结算(核销)优惠券
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<SettlementInfos> SettlementAsync(SettlementInfos info);

    }
}
