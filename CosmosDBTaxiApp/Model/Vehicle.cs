using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosDBTaxiApp.Model
{
    public class Vehicle : BaseModel
    {
        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; }
        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }
    }
}
