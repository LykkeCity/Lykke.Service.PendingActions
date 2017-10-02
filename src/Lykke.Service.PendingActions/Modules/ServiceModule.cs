using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.PendingActions.AzureRepositories.Clients;
using Lykke.Service.PendingActions.Core.Clients;
using Lykke.Service.PendingActions.Core.Services;
using Lykke.Service.PendingActions.Core.Settings.ServiceSettings;
using Lykke.Service.PendingActions.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.PendingActions.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<PendingActionsSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<PendingActionsSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterInstance<IClientPendingActionsRepository>(new ClientPendingActionsRepository(
                AzureTableStorage<PendingActionEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                    "ClientPendingActions", _log)));

            builder.Populate(_services);
        }
    }
}
