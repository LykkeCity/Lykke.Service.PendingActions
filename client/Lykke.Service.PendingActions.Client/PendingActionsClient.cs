using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PendingActions.AutorestClient;
using Lykke.Service.PendingActions.AutorestClient.Models;

namespace Lykke.Service.PendingActions.Client
{
    public class PendingActionsClient : IPendingActionsClient, IDisposable
    {
        private IPendingActionsAPI _apiClient;
        private readonly ILog _log;

        public PendingActionsClient(string serviceUrl, ILog log)
        {
            _log = log;
            _apiClient = new PendingActionsAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_apiClient == null)
                return;
            _apiClient.Dispose();
            _apiClient = null;
        }

        public async Task SetPendingAsync(string clientId, PendingActionType actionType, bool pending)
        {
            await _apiClient.ApiPendingActionsPostAsync(new SetPendingModel
            {
                ClientId = clientId,
                ActionType = actionType,
                Pending = pending
            });
        }

        public async Task<IEnumerable<PendingActionType>> GetPendingAsync(string clientId)
        {
            var response = await _apiClient.ApiPendingActionsGetAsync(clientId);

            var error = response as ErrorResponse;
            var result = response as PendingActionsResponse;

            if (result != null)
            {
                return result.PendingActions.Select(a => (PendingActionType) Enum.Parse(typeof(PendingActionType), a));
            }

            if (error != null)
            {
                await _log.WriteErrorAsync(GetType().Name, "GetPendingAsync",
                    $"clientId = {clientId}, error = {error.ErrorMessage}", null);
            }

            return null;
        }
    }
}
