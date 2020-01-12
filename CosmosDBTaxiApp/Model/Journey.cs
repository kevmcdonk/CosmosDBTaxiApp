using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosDBTaxiApp.Model
{
    public class Journey : BaseModel
    {
        [JsonProperty(PropertyName = "startPoint")]
        public Location StartPoint { get; set; }
        [JsonProperty(PropertyName = "endPoint")]
        public Location EndPoint { get; set; }
        [JsonProperty(PropertyName = "vehicleUsed")]
        public Vehicle VehicleUsed { get; set; }
        [JsonProperty(PropertyName = "vehicleDriver")]
        public Driver VehicleDriver { get; set; }
        public DateTime JourneyDate { get; set; }
    }
}
