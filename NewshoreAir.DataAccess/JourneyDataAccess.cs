using Dapper;
using Microsoft.Data.Sqlite;
using NewshoreAir.Interface.DataAccess;
using NewshoreApi.Entities.Entities;

namespace NewshoreAir.DataAccess
{
    public class JourneyDataAccess : IJourneyDataAccess
    {
        public List<Journey> GetJourneys(string origin, string destination, int? maxFlights = null)
        {
            origin = origin.ToUpper();
            destination = destination.ToUpper();

            using (var connection = new SqliteConnection("Data Source=./NewshoreAirDataBase.db"))
            {
                connection.Open();

                string query = @"
                SELECT 
                    J.Id, J.Origin, J.Destination, J.Price,
                    F.Id, F.Origin, F.Destination, F.Price,
                    T.Id AS TransportId, T.TransportFlightCarrier AS FlightCarrier, T.TransportFlightNumber AS FlightNumber
                FROM Journey J
                LEFT JOIN JourneyFlight JF ON J.Id = JF.JourneyId
                LEFT JOIN Flight F ON JF.FlightId = F.Id
                LEFT JOIN Transport T ON F.Id = T.FlightId
                WHERE J.Origin = @Origin AND J.Destination = @Destination";

                if (maxFlights.HasValue)
                {
                    query += " AND (SELECT COUNT(*) FROM JourneyFlight WHERE JourneyId = J.Id) <= @MaxFlights";
                }

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
                            flight.Transport = transport;
                            currentJourney.Flights.Add(flight);
                        }

                        return currentJourney;
                    },
                    new { Origin = origin, Destination = destination, MaxFlights = maxFlights },
                    splitOn: "Id,Id,TransportId"
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
                        var journeyId = connection.ExecuteScalar<int>(@"
                        INSERT INTO Journey (Origin, Destination, Price) 
                        VALUES (@Origin, @Destination, @Price);
                        SELECT last_insert_rowid();",
                            new { journey.Origin, journey.Destination, journey.Price });

                        foreach (var flight in journey.Flights)
                        {
                            var flightId = connection.ExecuteScalar<int>(@"
                        INSERT INTO Flight (TransportFlightCarrier, TransportFlightNumber, Origin, Destination, Price)
                        VALUES (@FlightCarrier, @FlightNumber, @Origin, @Destination, @Price);
                        SELECT last_insert_rowid();",
                                new { flight.Transport.FlightCarrier, flight.Transport.FlightNumber, flight.Origin, flight.Destination, flight.Price });

                            connection.Execute(@"
                        INSERT INTO Transport (FlightId, TransportFlightCarrier, TransportFlightNumber) 
                        VALUES (@FlightId, @FlightCarrier, @FlightNumber);
                        SELECT last_insert_rowid();",
                                new { FlightId = flightId, flight.Transport.FlightCarrier, flight.Transport.FlightNumber });

                            connection.Execute(@"
                        INSERT INTO JourneyFlight (JourneyId, FlightId) 
                        VALUES (@JourneyId, @FlightId);",
                                new { JourneyId = journeyId, FlightId = flightId });
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
