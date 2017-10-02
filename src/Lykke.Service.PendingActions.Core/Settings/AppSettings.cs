using Lykke.Service.PendingActions.Core.Settings.ServiceSettings;
using Lykke.Service.PendingActions.Core.Settings.SlackNotifications;

namespace Lykke.Service.PendingActions.Core.Settings
{
    public class AppSettings
    {
        public PendingActionsSettings PendingActionsService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
