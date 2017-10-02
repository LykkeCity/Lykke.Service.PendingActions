using System.ComponentModel.DataAnnotations;
using Lykke.Service.PendingActions.Core.Clients;

namespace Lykke.Service.PendingActions.Models
{
    public class SetPendingModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public PendingActionType ActionType { get; set; }
        [Required]
        public bool Pending { get; set; }
    }
}
