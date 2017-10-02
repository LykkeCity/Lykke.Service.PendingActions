using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PendingActions.Core.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PendingActions.AzureRepositories.Clients
{
    public class PendingActionEntity : TableEntity, IClientPendingAction
    {
        public static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }

        public static string GenerateRowKey(PendingActionType actionType)
        {
            return actionType.ToString();
        }

        public bool Pending { get; set; }
        public string ClientId => PartitionKey;
        public PendingActionType ActionType => (PendingActionType) Enum.Parse(typeof(PendingActionType), RowKey);

        public static PendingActionEntity Create(string clientId, PendingActionType actionType, bool pending)
        {
            return new PendingActionEntity
            {
                PartitionKey = GeneratePartitionKey(clientId),
                RowKey = GenerateRowKey(actionType),
                Pending = pending
            };
        }
    }

    public class ClientPendingActionsRepository : IClientPendingActionsRepository
    {
        private readonly INoSQLTableStorage<PendingActionEntity> _tableStorage;

        public ClientPendingActionsRepository(INoSQLTableStorage<PendingActionEntity> pendingActionsTableStorage)
        {
            _tableStorage = pendingActionsTableStorage;
        }

        public async Task SetPending(string clientId, PendingActionType actionType, bool flag)
        {
            await _tableStorage.InsertOrMergeAsync(PendingActionEntity.Create(clientId, actionType, flag));
        }

        public async Task<IEnumerable<IClientPendingAction>> GetPending(string clientId)
        {
            return (await _tableStorage.GetDataAsync(PendingActionEntity.GeneratePartitionKey(clientId))).Where(a =>
                a.Pending);
        }
    }
}
