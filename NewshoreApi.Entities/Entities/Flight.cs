using Newtonsoft.Json;

namespace NewshoreApi.Entities.Entities
{
    public class Flight
    {
        public int Id { get; set; }

        public Transport Transport { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }
    }
}
