using Newtonsoft.Json;

namespace NewshoreApi.Entities.Entities
{
    public class Transport
    {
        [JsonProperty(Order = 1)]
        public string FlightCarrier { get; set; }
        [JsonProperty(Order = 2)]
        public string FlightNumber { get; set; }
    }
}
