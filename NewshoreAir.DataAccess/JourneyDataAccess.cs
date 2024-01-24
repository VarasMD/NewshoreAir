using Dapper;
using Microsoft.Data.Sqlite;
using NewshoreAir.Interface.DataAccess;
using NewshoreApi.Entities.Entities;

namespace NewshoreAir.DataAccess
{
    public class JourneyDataAccess : IJourneyDataAccess
    {
        public List<Journey> GetJourneys(string origin, string destination)
        {
            using (var connection = new SqliteConnection("Data Source=./NewshoreAirDataBase.db"))
            {
                connection.Open();

                // Consulta SQL con condiciones WHERE para filtrar por origen y destino
                string query = @"
               SELECT 
                    J.Id, J.Origin, J.Destination, J.Price,
                    F.Id, F.TransportFlightCarrier, F.TransportFlightNumber, F.Origin AS FlightOrigin, F.Destination AS FlightDestination, F.Price AS FlightPrice,
                    T.Id AS TransportId, T.TransportFlightCarrier AS FlightCarrier, T.TransportFlightNumber AS FlightNumber
                FROM Journey J
                LEFT JOIN JourneyFlight JF ON J.Id = JF.JourneyId
                LEFT JOIN Flight F ON JF.FlightId = F.Id
                LEFT JOIN Transport T ON F.Id = T.FlightId
                WHERE J.Origin = @Origin AND J.Destination = @Destination";

                var journeyDictionary = new Dictionary<int, Journey>();

                var result = connection.Query<Journey, Flight, Transport, Journey>(
                    query,
                    (journey, flight, transport) =>
                    {
                        if (!journeyDictionary.TryGetValue(journey.Id, out var currentJourney))
                        {
                            currentJourney = journey;
                            currentJourney.Flights = new List<Flight>();
                            journeyDictionary.Add(currentJourney.Id, currentJourney);
                        }

                        if (flight != null)
                        {
                            flight.Transport = transport; // Asignar el transporte al vuelo
                            currentJourney.Flights.Add(flight);
                        }

                        return currentJourney;
                    },
                    new { Origin = origin, Destination = destination }, // Parámetros para la consulta SQL
                    splitOn: "Id,Id,TransportId" // Ajustar el orden de splitOn
                );


                return result.Distinct().ToList();
            }
        }

        public void SaveJourney(Journey journey)
        {
            using (var connection = new SqliteConnection("Data Source=./NewshoreAirDataBase.db"))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertar Journey
                        var journeyId = connection.ExecuteScalar<int>(@"
                        INSERT INTO Journey (Origin, Destination, Price) 
                        VALUES (@Origin, @Destination, @Price);
                        SELECT last_insert_rowid();",
                            new { journey.Origin, journey.Destination, journey.Price });

                        // Insertar JourneyFlight
                        foreach (var flight in journey.Flights)
                        {
                            // Insertar Flight
                            var flightId = connection.ExecuteScalar<int>(@"
                        INSERT INTO Flight (TransportFlightCarrier, TransportFlightNumber, Origin, Destination, Price)
                        VALUES (@FlightCarrier, @FlightNumber, @Origin, @Destination, @Price);
                        SELECT last_insert_rowid();",
                                new { flight.Transport.FlightCarrier, flight.Transport.FlightNumber, flight.Origin, flight.Destination, flight.Price });

                            // Insertar Transport
                            var transportId = connection.ExecuteScalar<int>(@"
                        INSERT INTO Transport (FlightId, TransportFlightCarrier, TransportFlightNumber) 
                        VALUES (@FlightId, @FlightCarrier, @FlightNumber);
                        SELECT last_insert_rowid();",
                                new { FlightId = flightId, flight.Transport.FlightCarrier, flight.Transport.FlightNumber });

                            // Insertar JourneyFlight
                            connection.Execute(@"
                        INSERT INTO JourneyFlight (JourneyId, FlightId) 
                        VALUES (@JourneyId, @FlightId);",
                                new { JourneyId = journeyId, FlightId = flightId });
                        }

                        // Confirmar la transacción si todo ha ido bien
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Rollback en caso de error
                        transaction.Rollback();
                        throw; // Relanzar la excepción para que se propague hacia arriba
                    }
                }
            }
        }
    }
}
