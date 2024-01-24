using Microsoft.Extensions.Logging;
using Moq;
using NewshoreAir.Business;
using NewshoreAir.Interface.DataAccess;
using NewshoreAir.Interface.Gateway;
using NewshoreApi.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JourneyManagerTests
{
    public class JourneyManagerTests
    {
        [Fact]
        public void GetJourneysReturned()
        {
            // Arrange
            var journeys = new List<Journey>
            {
                new Journey
                {
                    Id = 1,
                    Flights = new List<Flight>
                    {
                        new Flight
                        {
                            Id = 101,
                            Transport = new Transport
                            {
                                FlightCarrier = "AV",
                                FlightNumber = "123"
                            },
                            Origin = "ABC",
                            Destination = "XYZ",
                            Price = 500.0
                        }
                    },
                    Origin = "ABC",
                    Destination = "XYZ",
                    Price = 500.0
                },
            };

            var mockJourneyDataAccess = new Mock<IJourneyDataAccess>();
            mockJourneyDataAccess.Setup(x => x.GetJourneys(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(journeys);

            var mockRouteGateway = new Mock<IRouteGateway>();

            var mockLogging = new Mock<ILogger<JourneyBusiness>>();

            var journeyBusiness = new JourneyBusiness(mockJourneyDataAccess.Object, mockRouteGateway.Object, mockLogging.Object);

            // Act
            var result = journeyBusiness.GetJourneys("Origin", "Destination");

            // Assert
            Xunit.Assert.Same(journeys, result); // Asegura que se devuelvan los datos de la base de datos
        }

        [Fact]
        public void GetJourneysValidRoutesFound()
        {
            // Arrange
            var mockJourneyDataAccess = new Mock<IJourneyDataAccess>();
            mockJourneyDataAccess.Setup(x => x.GetJourneys(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(new List<Journey>());

            var expectedRoutes = new List<Route>
            {
                new Route
                {
                    DepartureStation = "ABC",
                    ArrivalStation = "XYZ",
                    FlightCarrier = "AV",
                    FlightNumber = "123",
                    Price = 500.0
                },
                new Route
                {
                    DepartureStation = "XYZ",
                    ArrivalStation = "DEF",
                    FlightCarrier = "AV",
                    FlightNumber = "456",
                    Price = 600.0
                },
            };

            var mockRouteGateway = new Mock<IRouteGateway>();
            mockRouteGateway.Setup(x => x.GetRoutes()).ReturnsAsync(expectedRoutes);

            var mockLogging = new Mock<ILogger<JourneyBusiness>>();

            var journeyBusiness = new JourneyBusiness(mockJourneyDataAccess.Object, mockRouteGateway.Object, mockLogging.Object);

            // Act & Assert
            Xunit.Assert.Throws<JourneyBusiness.NoFlightsFoundException>(() =>
            {
                var result = journeyBusiness.GetJourneys("Origin", "Destination");
            });

            mockJourneyDataAccess.Verify(x => x.SaveJourney(It.IsAny<List<Journey>>()), Times.Never);
        }
    }
}
