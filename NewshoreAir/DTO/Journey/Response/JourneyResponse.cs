using NewshoreAir.DTO.Flight.Response;

namespace NewshoreAir.DTO.Journey.Response
{
    public class JourneyResponse
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public List<FlightResponse> Flights { get; set; }
        
    }
}
