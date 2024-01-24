using NewshoreAir.DTO.Transport.Response;

namespace NewshoreAir.DTO.Flight.Response
{
    public class FlightResponse
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public TransportResponse Transport { get; set; }
        
    }
}
