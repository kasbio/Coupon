using Kasbio.Coupon.Common.Utilities;
using Kasbio.Coupon.Template.ApplicationCore.Entities;
using Kasbio.Coupon.Template.ApplicationCore.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Kasbio.Coupon.Template.ApplicationCore.Tasks
{
    public class CheckExpiredCouponTemplateTask : BackgroundService
    {

        private ILogger<CheckExpiredCouponTemplateTask> _logger;
        private ITemplateRepository _template;

        public CheckExpiredCouponTemplateTask(ILogger<CheckExpiredCouponTemplateTask> logger, ITemplateRepository template)
        {
            _logger = logger;
            _template = template;
        }

        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    stoppingToken.ThrowIfCancellationRequested();
                }
                _logger.LogInformation("start to expire template");

                //获取所有未过期优惠券模板
                var templates = await _template.GetAllByExpied(false);

                DateTime now = DateTime.Now;
                var expiredTemplates = new List<CouponTemplate>();
                templates.ForEach(o =>
                {
                    if (o.Rule.Expiration.DeadLine < DateTimeUtility.ConvertToTimeStamp(now))
                    {
                        o.Expired = true;
                        expiredTemplates.Add(o);
                    }
                });


                if (expiredTemplates.Count != 0)
                {
                    int i = await _template.UpdateRangeAsync(expiredTemplates.ToArray());
                    _logger.LogInformation($"Expired CouponTemplate Num: {i}");
                }

                _logger.LogInformation("Done To Expire CouponTemplate.");

            }
            catch (Exception e )
            {

                _logger.LogError(e.Message);
            }
      





        }
    }
}
