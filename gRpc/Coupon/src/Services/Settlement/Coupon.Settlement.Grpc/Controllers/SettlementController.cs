using Coupon.Settlement.ApplicationCore.Executor;
using Coupon.Settlement.Grpc.Model;
using Grpc.Core;

namespace Coupon.Settlement.Grpc.Controllers;

public class SettlementController:SettlementServices.SettlementServicesBase
{
    private readonly RuleExecutor _executor;
    private readonly ILogger<SettlementController> _logger;
    private readonly RuleService ruleService;
    public SettlementController(RuleExecutor executor, 
        ILogger<SettlementController> logger, RuleService ruleService)
    {
        _executor = executor;
        _logger = logger;
        this.ruleService = ruleService;
    }

    public override async Task<SettlementInfos> ComputeRule(SettlementInfos request, ServerCallContext context)
    {
        var result = await ruleService.Compute(request);
        return result;
        
    }
}