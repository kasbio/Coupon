using Coupon.Template.DbContexts;
using Coupon.Template.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Coupon.Template.Infrastructure.Tasks
{

    /// <summary>
    /// TODO：应改成后台定时任务
    /// </summary>
    internal class CheckExpiredCouponTemplateTask : BackgroundService
    {

        private ILogger<CheckExpiredCouponTemplateTask> _logger;
        private TemplateDbContext _template;

        public CheckExpiredCouponTemplateTask(ILogger<CheckExpiredCouponTemplateTask> logger,
            TemplateDbContext template)
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
                var templates = await _template.GetAllByExpiedAsync(false);

                DateTime now = DateTime.Now;
                var expiredTemplates = new List<CouponTemplate>();
                templates.ForEach(o =>
                {
                    if (o.Rule.Expiration.Deadline < now.ConvertToTimeStamp())
                    {
                        o.Expired = true;
                        expiredTemplates.Add(o);
                    }
                });


                if (expiredTemplates.Count != 0)
                {
                    _template.UpdateRange(expiredTemplates.ToArray());
                }
                await _template.SaveChangesAsync();
                _logger.LogInformation($"Done To Expire CouponTemplate.Count{expiredTemplates.Count()}");

            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }






        }
    }
}
