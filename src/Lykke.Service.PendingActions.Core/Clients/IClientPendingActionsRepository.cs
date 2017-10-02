using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PendingActions.Core.Clients
{
    public enum PendingActionType
    {
        UnsignedTransaction,
        OffchainRequest,
        ClientDialog
    }

    public interface IClientPendingAction
    {
        string ClientId { get; }
        PendingActionType ActionType { get; }
        bool Pending { get; }
    }

    public interface IClientPendingActionsRepository
    {
        Task SetPending(string clientId, PendingActionType actionType, bool flag);
        Task<IEnumerable<IClientPendingAction>> GetPending(string clientId);
    }
}
