using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDBTaxiApp.Model;

namespace CosmosDBTaxiApp.Services
{
    public interface ICosmosDBService<T> where T : BaseModel
    {
        public Task<T> GetItemAsync(string id);
        public Task<T> AddItemAsync(T item);
        public Task<T> DeleteItemAsync(string id);
        public Task<IEnumerable<T>> GetItemsAsync(string queryString);
        public Task<T> UpdateItemAsync(T item);

    }
}
