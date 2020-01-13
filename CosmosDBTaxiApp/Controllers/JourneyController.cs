using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CosmosDBTaxiApp.Model;
using CosmosDBTaxiApp.Services;
using Microsoft.AspNet.OData;


namespace CosmosDBTaxiApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JourneyController : ControllerBase
    {
        private readonly ICosmosDBService<Journey> _journeyService;
        public JourneyController(ICosmosDBService<Journey> journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpGet]
        [EnableQuery()]
        public IEnumerable<Journey> Get()
        {
            return _journeyService.GetItemsAsync("select * from c").GetAwaiter().GetResult();
        }

        [HttpPost]
        public Journey Post(Journey journey)
        {
            Journey createdJourney = _journeyService.AddItemAsync(journey).GetAwaiter().GetResult();
            return createdJourney;
        }

        [HttpPut]
        public Journey Put(Journey journey)
        {
            Journey createdJourney = _journeyService.UpdateItemAsync(journey).GetAwaiter().GetResult();
            return createdJourney;
        }

        [HttpDelete]
        public Journey Delete(Journey journey)
        {
            Journey createdJourney = _journeyService.DeleteItemAsync(journey.Id.ToString()).GetAwaiter().GetResult();
            return createdJourney;
        }
    }
}
