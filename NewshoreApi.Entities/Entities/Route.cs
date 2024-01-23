namespace NewshoreApi.Entities.Entities
{
    public class Route
    {
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }
        public double Price { get; set; }

        public Flight ConvertToFlight()
        {
            return new Flight
            {
                Transport = new Transport
                {
                    FlightCarrier = this.FlightCarrier,
                    FlightNumber = this.FlightNumber
                },
                Origin = this.DepartureStation,
                Destination = this.ArrivalStation,
                Price = this.Price
            };
        }
    }
}
