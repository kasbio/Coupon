using Coupon.Settlement.ApplicationCore.Executor;
using Coupon.Settlement.Grpc.Model;
using Coupon.Template.Grpc;

namespace Coupon.Settlement
{
    public class RuleService
    {
        private readonly CouponTemplateServices.CouponTemplateServicesClient _templateClient;
        private readonly RuleExecutor _executor;

        public RuleService(CouponTemplateServices.CouponTemplateServicesClient templateClient, RuleExecutor executor)
        {
            _templateClient = templateClient;
            _executor = executor;
        }

        public async Task<SettlementInfos> Compute(SettlementInfos request)
        {
            var ids = request.CouponInfos.Select(i => i.Template.Id).ToArray();

            var collection = new Template.Grpc.Model.CouponTemplateIds();
            collection.Ids.AddRange(ids);
            var tResult = await _templateClient.FindId2TemplateSDKAsync(collection);
            if (tResult.Code == 0)
            {
                foreach (var item in tResult.Data)
                {
                    var c = request.CouponInfos.FirstOrDefault(i => i.Template.Id == item.Id);
                    c.Template = item;
                }

            }

            return _executor.ComputeRule(request);
        }


    }
}
