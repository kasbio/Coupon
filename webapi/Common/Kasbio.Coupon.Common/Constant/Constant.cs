using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.Constant
{
    public class Constant
    {

        /** Kafka 消息的Topic */
        public static string TOPIC = "kasbio_user_coupon_op";

        

        public  class RedisPrefix
        {

            /** 优惠券吗key 前缀 */
            public static string COUPON_TEMPLATE
            = "kasbio_coupon_template_code_";
            /** 用户当前所有可用的优惠券key 前缀*/
            public static string USER_COUPON_USABLE =
                    "kasbio_user_coupon_usable_";
            /**  用户当前所有已使用的优惠券 key 前缀*/
            public static string USER_COUPON_USED =
                    "kasbio_user_coupon_used_";
            /** 用户当前所有已过期的优惠券 key 前缀 */
            public static string USER_COUPON_EXPIRED =
                    "kabsio_user_coupon_expired_";
        }




    }

}
