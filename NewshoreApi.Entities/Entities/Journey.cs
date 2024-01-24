using Newtonsoft.Json;

namespace NewshoreApi.Entities.Entities
{
    public class Journey
    {
        public int Id { get; set; }

        [JsonProperty(Order = 1)]
        public List<Flight> Flights { get; set; }

        [JsonProperty(Order = 2)]
        public string Origin { get; set; }

        [JsonProperty(Order = 3)]
        public string Destination { get; set; }

        [JsonProperty(Order = 4)]
        public double Price { get; set; }
    }
}
