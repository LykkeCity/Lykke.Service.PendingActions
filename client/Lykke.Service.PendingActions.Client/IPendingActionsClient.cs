using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.PendingActions.AutorestClient.Models;

namespace Lykke.Service.PendingActions.Client
{
    public interface IPendingActionsClient
    {
        Task SetPendingAsync(string clientId, PendingActionType actionType, bool pending);
        Task<IEnumerable<PendingActionType>> GetPendingAsync(string clientId);
    }
}
