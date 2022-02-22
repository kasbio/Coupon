using Coupon.Distribution.Model.Contant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Distribution.ApplicationCore.Services
{
    public interface IRedisService
    {
        /// <summary>
        /// 根据userId 和状态找到缓存的优惠券列表数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<Model.Coupon>> GetCacheCouponsAsync(long userId, CouponStatus status);

        /// <summary>
        /// 保持空的优惠券列表到缓存中【缓存穿透处理】
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        Task SaveEmptyCouponListToCacheAsync(long userId, params CouponStatus[] status);
        /// <summary>
        /// 尝试从Cache 中获取一个优惠券码
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Task<string> TryToAcquireCouponCodeFromCacheAsync(int templateId);

        /// <summary>
        /// 将优惠券保存到Cache中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="coupons"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> AddCouponToCacheAsync(long userId, IEnumerable<Model.Coupon> coupons
                , CouponStatus status);

        /// <summary>
        /// 结算(核销)优惠券
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
       // SettlementInfo Settlement(SettlementInfo info);
    }
}
