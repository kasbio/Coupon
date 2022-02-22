using Kasbio.Coupon.Common.Constant;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Kasbio.Coupon.Template.ApplicationCore.Command.Handler
{
    public class ConstructCouponByTemplateHandler : IRequestHandler<ConstructCouponByTemplateCommand>
    {
        private ILogger<ConstructCouponByTemplateHandler> _log;
        private ITemplateRepository _templateRepository;


        public ConstructCouponByTemplateHandler(ILogger<ConstructCouponByTemplateHandler> log, ITemplateRepository templateRepository)
        {
            _log = log;
            _templateRepository = templateRepository;
        }


        public async Task<Unit> Handle(ConstructCouponByTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = request.Template;
            string redisKey = Constant.RedisPrefix.COUPON_TEMPLATE + request.Template.Id.ToString("D4");
            _log.LogDebug($"redis key: {redisKey}");
            var codes = BuildCouponCode(template);
       


            long i =  RedisHelper.RPush(redisKey, codes.ToArray());
            _log.LogDebug($"success insert coupon code : {i}");
            template.Available = true;
            await _templateRepository.UpdateAsync(template);

            return Unit.Value;


        }


        private HashSet<string> BuildCouponCode(CouponTemplate template)
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            HashSet<string> result = new HashSet<string>(template.Count);

            // 前四位
            string prefix4 = template.ProductLine.Code.ToString()
                    + template.Category.Code;
            string date = template.CreateTime.ToString("yyMMdd");

            for (int i = 0; i != template.Count; ++i)
            {
                result.Add(prefix4 + BuildCouponCodeSuffix14(date));
            }

            while (result.Count < template.Count)
            {
                result.Add(prefix4 + BuildCouponCodeSuffix14(date));
            }

            if (result.Count == template.Count)
            {

            }

            watch.Stop();
            _log.LogInformation("Build Coupon Code Cost: {}ms",
                    watch.ElapsedMilliseconds);

            return result;
        }



        private string BuildCouponCodeSuffix14(string date)
        {
            // 中间六位
            char[] chars = date.ToCharArray();
            //打乱
            Random r = new Random();//创建随机类对象，定义引用变量为r
            for (int i = 0; i < chars.Length; i++)
            {
                int index = r.Next(chars.Length);//随机获得0（包括0）到arr.Length（不包括arr.Length）的索引
                //Console.WriteLine("index={0}", index);//查看index的值
                char temp = chars[i];  //当前元素和随机元素交换位置
                chars[i] = chars[index];
                chars[index] = temp;
            }

            //中间六位
            string mid6 = new string(chars, 0, chars.Length);


            string suffix8 = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                suffix8 += r.Next(0, 10).ToString();
            }


            return mid6 + suffix8;
        }
    }
}
