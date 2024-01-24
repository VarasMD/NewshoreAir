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
            var journeysFromDatabase = _journeyDataAccess.GetJourneys(origin, destination); //podria buscar especificamente origen y destino y evitar el siguiente paso

            // Lógica para verificar si la ruta ya ha sido calculada
            //var existingJourneys = journeysFromDatabase
            //.Where(journey => journey.Origin == origin && journey.Destination == destination && (!maxFlights.HasValue || journey.Flights.Count <= maxFlights))
            //.ToList();

            if (journeysFromDatabase.Count > 0)
            {
                // Rutas previamente calculadas, devolverlas
                return journeysFromDatabase;
            }

            // Ruta no encontrada en la base de datos, calcularla
            var routes = _routeGateway.GetRoutes().Result;
            var journeys = FindJourneys(origin, destination, routes, new List<Flight>(), new HashSet<string>(), maxFlights);

            if (journeys.Count == 0)
            {
                // No se encontraron vuelos para este viaje, lanzar excepción
                throw new NoFlightsFoundException();
            }

            // Guardar la nueva ruta en la base de datos
            foreach (var journey in journeys)
            {
                _journeyDataAccess.SaveJourney(journey);
            }

            return journeys;
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
                    // Encontrada una ruta completa
                    // Verificar la cantidad de vuelos en la ruta
                    if (!maxFlights.HasValue || currentPath.Count <= maxFlights)
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
                }
                else
                {
                    if (!maxFlights.HasValue || currentPath.Count < maxFlights)
                    {
                        // Continuar la búsqueda recursiva con la restricción de cantidad máxima de vuelos
                        var nextRoutes = FindJourneys(route.ArrivalStation, destination, routes, currentPath, visited, maxFlights);
                        validRoutes.AddRange(nextRoutes);
                    }
                }

                // Deshacer el cambio para probar otras rutas
                currentPath.Remove(currentPath.Last());
            }

            visited.Remove(currentLocation);

            return validRoutes;
        }
        #endregion
    }
}




