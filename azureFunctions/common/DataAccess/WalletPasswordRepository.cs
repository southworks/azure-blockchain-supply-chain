using Microsoft.Azure.Cosmos;
using SupplyChain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.Common.DataAccess
{
    public class WalletPasswordRepository : IWalletPasswordRepository
    {
        private Container _container;

        public WalletPasswordRepository()
        {
            Task.Run(() => this.Initialize()).Wait();
        }

        public async Task Initialize()
        {
            try
            {
                var _cosmosClient = new CosmosClient(EnvironmentVariables.CosmosEndpointUri, EnvironmentVariables.CosmosPrimaryKey);

                var databaseId = EnvironmentVariables.CosmosDatabaseId;
                var containerId = EnvironmentVariables.WalletContainerId;

                await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

                var database = _cosmosClient.GetDatabase(databaseId);

                await database.CreateContainerIfNotExistsAsync(containerId, "/id");

                _container = _cosmosClient.GetContainer(databaseId, containerId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Initializing Cosmos DB failed with error: {ex}");
            }
        }

        public async Task Insert(WalletPassword walletPassword)
        {
            try
            {
                await _container.CreateItemAsync<WalletPassword>(walletPassword, new PartitionKey(walletPassword.Id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Inserting wallet password into Cosmos DB failed with error: {ex}");
            }
        }

        public async Task<WalletPassword> GetById(string id)
        {
            var query = $"SELECT * FROM c WHERE c.id = \"{id}\"";

            return await GetByQuery(query);
        }

        public async Task<WalletPassword> GetByUserId(string id)
        {
            var query = $"SELECT * FROM c WHERE c.userProfileId = \"{id}\"";

            return await GetByQuery(query);
        }

        public async Task Delete(string id)
        {
            await _container.DeleteItemAsync<WalletPassword>(id, new PartitionKey(id));
        }

        public async Task Update(string id, WalletPassword walletPassword)
        {
            await _container.ReplaceItemAsync<WalletPassword>(walletPassword, id, new PartitionKey(id));
        }

        private async Task<WalletPassword> GetByQuery(string query)
        {
            var queryDefinition = new QueryDefinition(query);

            var results = new List<WalletPassword>();

            var iterator = _container.GetItemQueryIterator<WalletPassword>(queryDefinition);

            foreach (var result in await iterator.ReadNextAsync())
            {
                results.Add(result);
            }

            return results.FirstOrDefault();
        }
    }
}
