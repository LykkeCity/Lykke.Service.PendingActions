using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.PendingActions.Core.Clients;
using Lykke.Service.PendingActions.Core.Filter;
using Lykke.Service.PendingActions.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.PendingActions.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class PendingActionsController : Controller
    {
        private readonly IClientPendingActionsRepository _pendingActionsRepository;
        private readonly ILog _log;

        public PendingActionsController(IClientPendingActionsRepository pendingActionsRepository, ILog log)
        {
            _pendingActionsRepository = pendingActionsRepository;
            _log = log;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PendingActionsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPendingActionsAsync([FromQuery] string clientId)
        {
            try
            {
                var pendingActions = await _pendingActionsRepository.GetPending(clientId);

                return Ok(new PendingActionsResponse
                {
                    PendingActions = pendingActions.Select(a => a.ActionType)
                });
            }
            catch (Exception e)
            {
                await _log.WriteErrorAsync(GetType().Name, "GetPendingActionsAsync", $"clientId = {clientId}", e,
                    DateTime.UtcNow);
                return BadRequest(ErrorResponse.Create(e.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetPendingAsync([FromBody] SetPendingModel model)
        {
            try
            {
                await _pendingActionsRepository.SetPending(model.ClientId, model.ActionType, model.Pending);
            }
            catch (Exception e)
            {
                await _log.WriteErrorAsync(GetType().Name, "SetPendingAsync", model.ToJson(), e,
                    DateTime.UtcNow);
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return Ok();
        }
    }
}
