using Microsoft.Extensions.Logging;
using NewshoreAir.Interface.Business;
using NewshoreAir.Interface.DataAccess;
using NewshoreAir.Interface.Gateway;
using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Business
{
    public class JourneyBusiness : IJourneyBusiness
    {
        #region Private Fields
        private readonly IJourneyDataAccess _journeyDataAccess;
        private readonly IRouteGateway _routeGateway;
        private readonly ILogger<JourneyBusiness> _logger;
        #endregion

        #region Constructor
        public JourneyBusiness(IJourneyDataAccess journeyDataAccess, IRouteGateway routeGateway, ILogger<JourneyBusiness> logger)
        {
            _journeyDataAccess = journeyDataAccess;
            _routeGateway = routeGateway;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public List<Journey> GetJourneys(string origin, string destination, int? maxFlights = null)
        {
            _logger.LogInformation($"Starting journeys search from {origin} to {destination}.");
            var journeysFromDatabase = _journeyDataAccess.GetJourneys(origin, destination, maxFlights);

            if (journeysFromDatabase.Count > 0)
            {
                _logger.LogInformation($"Flight search successfully completed in DataBase");
                return journeysFromDatabase;
            }

            var routes = _routeGateway.GetRoutes().Result;
            var journeys = FindJourneys(origin, destination, routes, new List<Flight>(), new HashSet<string>());
            var journeysResult = FilterJourneysByMaxFlights(journeys, maxFlights);

            if (journeysResult.Count == 0)
            {
                _logger.LogWarning($"No flights were found for the trip from {origin} to {destination}.");
                throw new NoFlightsFoundException();
            }

            _logger.LogInformation($"Flight search successfully completed.");

            _journeyDataAccess.SaveJourney(journeys);

            return journeysResult;
        }

        public class NoFlightsFoundException : Exception
        {
            public NoFlightsFoundException() : base("No flights were found for this trip.")
            {
            }
        }
        #endregion

        #region Private Methods
        private List<Journey> FindJourneys(string currentLocation, string destination, List<Route> routes, List<Flight> currentPath, HashSet<string> visited)
        {
            var validRoutes = new List<Journey>();

            visited.Add(currentLocation);

            foreach (var route in routes.Where(route => route.DepartureStation == currentLocation && !visited.Contains(route.ArrivalStation)))
            {
                currentPath.Add(route.ConvertToFlight());

                if (route.ArrivalStation == destination)
                {
                    var journey = new Journey
                    {
                        Origin = currentPath.First().Origin,
                        Destination = currentPath.Last().Destination,
                        Flights = currentPath.ToList(),
                        Price = currentPath.Sum(f => f.Price)
                    };
                    validRoutes.Add(journey);
                }
                else
                {
                     var nextRoutes = FindJourneys(route.ArrivalStation, destination, routes, currentPath, visited);
                     validRoutes.AddRange(nextRoutes);
                }

                currentPath.Remove(currentPath.Last());
            }

            visited.Remove(currentLocation);

            return validRoutes;
        }

        private List<Journey> FilterJourneysByMaxFlights(List<Journey> journeys, int? maxFlights)
        {
            if (maxFlights.HasValue && journeys.Count > 0)
            {
                return journeys.Where(journey => journey.Flights.Count <= maxFlights).ToList();
            }
            return journeys;
        }
        #endregion
    }
}




