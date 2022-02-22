using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Settlement.ApplicationCore.Executor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Settlement.Web.Controllers
{
    [ApiController]
    public class SettlementController : ControllerBase
    {

        private readonly ExecuteManager executeManager;
        private readonly ILogger<SettlementController> _logger;
        public SettlementController(ExecuteManager executeManager, ILogger<SettlementController> logger)
        {
            this.executeManager = executeManager;
            this._logger = logger;
        }


        [HttpPost("settlement/compute")]
        public SettlementInfo ComputeRule(SettlementInfo settlement)
        {
            _logger.LogInformation($"settlement: {JsonConvert.SerializeObject(settlement)}");
            return executeManager.ComputeRule(settlement);
        }


    }
}
