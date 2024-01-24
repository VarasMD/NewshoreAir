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
        #endregion

        #region Constructor
        public JourneyBusiness(IJourneyDataAccess journeyDataAccess, IRouteGateway routeGateway)
        {
            _journeyDataAccess = journeyDataAccess;
            _routeGateway = routeGateway;
        }
        #endregion

        #region Public Methods
        public List<Journey> GetJourneys(string origin, string destination, int? maxFlights = null)
        {
            var journeysFromDatabase = _journeyDataAccess.GetJourneys(origin, destination, maxFlights);

            if (journeysFromDatabase.Count > 0)
            {
                return journeysFromDatabase;
            }

            var routes = _routeGateway.GetRoutes().Result;
            var journeys = FindJourneys(origin, destination, routes, new List<Flight>(), new HashSet<string>());

            if (journeys.Count == 0)
            {
                throw new NoFlightsFoundException();
            }

            foreach (var journey in journeys)
            {
                _journeyDataAccess.SaveJourney(journey);
            }

            return FilterJourneysByMaxFlights(journeys, maxFlights);
        }

        public class NoFlightsFoundException : Exception
        {
            public NoFlightsFoundException() : base("No se encontraron vuelos para este viaje.")
            {
            }
        }
        #endregion

        #region Private Methods
        private List<Journey> FindJourneys(string currentLocation, string destination, List<Route> routes, List<Flight> currentPath, HashSet<string> visited, int? maxFlights = null)
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
                     var nextRoutes = FindJourneys(route.ArrivalStation, destination, routes, currentPath, visited, maxFlights);
                     validRoutes.AddRange(nextRoutes);
                }

                currentPath.Remove(currentPath.Last());
            }

            visited.Remove(currentLocation);

            return validRoutes;
        }

        private List<Journey> FilterJourneysByMaxFlights(List<Journey> journeys, int? maxFlights)
        {
            if (maxFlights.HasValue)
            {
                return journeys.Where(journey => journey.Flights.Count <= maxFlights).ToList();
            }
            return journeys;
        }
        #endregion
    }
}




