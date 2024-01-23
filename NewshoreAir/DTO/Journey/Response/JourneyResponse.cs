using NewshoreAir.DTO.Flight.Response;

namespace NewshoreAir.DTO.Journey.Response
{
    public class JourneyResponse
    {
        public List<FlightResponse> Flights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
