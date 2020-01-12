using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosDBTaxiApp.Model
{
    public class Location : BaseModel
    {
        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty(PropertyName = "addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonProperty(PropertyName = "town")]
        public string Town { get; set; }
        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }
}
