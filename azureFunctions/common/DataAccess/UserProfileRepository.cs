using Microsoft.Azure.Cosmos;
using SupplyChain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.Common.DataAccess
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private Container _container;

        public UserProfileRepository()
        {
            Task.Run(() => this.Initialize()).Wait();
        }

        public async Task Initialize()
        {
            try
            {
                var _cosmosClient = new CosmosClient(EnvironmentVariables.CosmosEndpointUri, EnvironmentVariables.CosmosPrimaryKey);

                var databaseId = EnvironmentVariables.CosmosDatabaseId;
                var containerId = EnvironmentVariables.UserContainerId;

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

        public async Task Insert(UserProfile userProfile)
        {
            try
            {
                await _container.CreateItemAsync<UserProfile>(userProfile, new PartitionKey(userProfile.Id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Inserting keyStore into Cosmos DB failed with error: {ex}");
            }
        }

        public async Task<UserProfile> GetById(string id)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = \"{id}\"";

            var queryDefinition = new QueryDefinition(sqlQueryText);

            var results = new List<UserProfile>();

            var iterator = _container.GetItemQueryIterator<UserProfile>(queryDefinition);

            foreach (var result in await iterator.ReadNextAsync())
            {
                results.Add(result);
            }

            return results.FirstOrDefault();
        }

        public async Task Delete(string id)
        {
            await _container.DeleteItemAsync<UserProfile>(id, new PartitionKey(id));
        }

        public async Task Update(string id, UserProfile userProfile)
        {
            await _container.ReplaceItemAsync<UserProfile>(userProfile, id, new PartitionKey(id));
        }
    }
}
