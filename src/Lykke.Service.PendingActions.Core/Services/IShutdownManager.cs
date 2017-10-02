using System.Threading.Tasks;

namespace Lykke.Service.PendingActions.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}