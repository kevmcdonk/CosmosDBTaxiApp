using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;
using System.Linq;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using CosmosDBTaxiApp.Model;


namespace CosmosDBTaxiApp.Services
{
    public class CosmosDBService<T> : ICosmosDBService<T> where T : BaseModel
    {

        private Container _container;
        public CosmosDBService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var document = await _container.ReadItemAsync<T>(id, new PartitionKey(typeof(T).ToString()));
                return (T)(dynamic)document;
            }
            catch (Microsoft.Azure.Cosmos.CosmosException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<T> AddItemAsync(T item)
        {
            try
            {
                ItemResponse<T> response = await this._container.CreateItemAsync<T>(item, new PartitionKey(item.Id.ToString()));
                return response.Resource;

            }
            catch (Microsoft.Azure.Cosmos.CosmosException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<T> DeleteItemAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await this._container.DeleteItemAsync<T>(id, new PartitionKey(typeof(T).ToString()));
                return response.Resource;
            }
            catch (Microsoft.Azure.Cosmos.CosmosException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task<T> UpdateItemAsync(T item)
        {
            try
            {
                ItemResponse<T> response = await this._container.UpsertItemAsync<T>(item, new PartitionKey(typeof(T).ToString()));
                return response.Resource;

            }
            catch (Microsoft.Azure.Cosmos.CosmosException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
        
    }
}
