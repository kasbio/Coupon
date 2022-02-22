using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coupon.Redis
{
    public sealed class Constant
    {

        /** Kafka 消息的Topic */
        public static string TOPIC = "kasbio_user_coupon_op";


        public static string GetCouponRedisKey(int id)
        {
           return RedisPrefix.COUPON_TEMPLATE + id.ToString("D4");
        }
        public class RedisPrefix
        {

            /// <summary>
            /// 优惠券吗key 前缀 
            /// </summary>
            public static string COUPON_TEMPLATE
            = "kasbio_coupon_template_code_";
            /// <summary>
            /// 用户当前所有可用的优惠券key 前缀
            /// </summary>
            public static string USER_COUPON_USABLE =
                    "kasbio_user_coupon_usable_";
            /// <summary>
            /// 用户当前所有已使用的优惠券 key 前缀
            /// </summary>
            public static string USER_COUPON_USED =
                    "kasbio_user_coupon_used_";
            /// <summary>
            /// 用户当前所有已过期的优惠券 key 前缀
            /// </summary>
            public static string USER_COUPON_EXPIRED =
                    "kabsio_user_coupon_expired_";
        }
    }
}
