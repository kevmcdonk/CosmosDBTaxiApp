using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDBTaxiApp.Model;

namespace CosmosDBTaxiApp.Services
{
    public interface ICosmosDBService<T> where T : BaseModel
    {
        Task<T> GetItemAsync(string id);
        Task<T> AddItemAsync(T item);
        Task<T> DeleteItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(string queryString);
        Task<T> UpdateItemAsync(T item);

    }
}
