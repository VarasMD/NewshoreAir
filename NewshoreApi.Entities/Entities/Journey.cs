using Newtonsoft.Json;

namespace NewshoreApi.Entities.Entities
{
    public class Journey
    {
        public int Id { get; set; }

        public List<Flight> Flights { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }
    }
}
