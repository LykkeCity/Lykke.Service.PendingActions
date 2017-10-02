using System.Collections.Generic;
using Lykke.Service.PendingActions.Core.Clients;

namespace Lykke.Service.PendingActions.Models
{
    public class PendingActionsResponse
    {
        public IEnumerable<PendingActionType> PendingActions { get; set; }
    }
}
