using NewshoreAir.DTO.Transport.Response;

namespace NewshoreAir.DTO.Flight.Response
{
    public class FlightResponse
    {
        public TransportResponse Transport { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
