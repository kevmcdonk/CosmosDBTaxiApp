using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosDBTaxiApp.Model
{
    public class Driver : BaseModel
    {
        [JsonProperty(PropertyName = "driverLicenseNumber")]
        public string DriverLicenseNumber;
        [JsonProperty(PropertyName = "age")]
        public int Age;
        [JsonProperty(PropertyName = "homeLocation")]
        public Location HomeLocation;
    }
}
