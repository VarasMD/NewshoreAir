﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewshoreAir.Controllers;
using NewshoreAir.DTO.Journey.Response;
using NewshoreApi.Entities.Entities;
using AutoMoqCore;
using static NewshoreAir.Business.JourneyBusiness;
using NewshoreAir.Interface.Business;
using Moq;
using NewshoreAir.DTO.Transport.Response;
using NewshoreAir.DTO.Flight.Response;
using System.Net;
using Microsoft.Extensions.Logging;
using NewshoreAir.Business;

namespace JourneyControllerTests
{
    public class JourneyControllerTests
    {
        private readonly AutoMoqer _autoMoqer;
        private readonly JourneyController _journeyController;

        public JourneyControllerTests()
        {
            _autoMoqer = new AutoMoqer();
            _journeyController = _autoMoqer.Resolve<JourneyController>();
        }

        [Fact]
        public void GetJourneyResultoOK()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockJourneyBusiness = new Mock<IJourneyBusiness>();
            var mockLogging = new Mock<ILogger<JourneyController>>();
            var controller = new JourneyController(mockMapper.Object, mockJourneyBusiness.Object, mockLogging.Object);

            var origin = "Origin";
            var destination = "Destination";
            var maxFlights = 2;

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

            var journeysResponse = new List<JourneyResponse>
            {
                new JourneyResponse
                {
                    Flights = new List<FlightResponse>
                    {
                        new FlightResponse
                        {
                            Transport = new TransportResponse
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

            mockJourneyBusiness
                .Setup(x => x.GetJourneys(origin, destination, maxFlights))
                .Returns(journeys);

            mockMapper
                .Setup(x => x.Map<List<JourneyResponse>>(journeys))
                .Returns(journeysResponse);

            // Act
            var result = controller.GetJourney(origin, destination, maxFlights);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<JourneyResponse>>(okResult.Value);
            Assert.Equal(journeys.Count, model.Count);
        }

        [Fact]
        public void GetJourneyNoFlightsFound()
        {
            // Arrange
            var mockJourneyBusiness = _autoMoqer.GetMock<IJourneyBusiness>();

            var origin = "Origin";
            var destination = "Destination";
            var maxFlights = 2;

            mockJourneyBusiness
                .Setup(x => x.GetJourneys(origin, destination, maxFlights))
                .Throws(new JourneyBusiness.NoFlightsFoundException());

            // Act
            var result = _journeyController.GetJourney(origin, destination, maxFlights).Result;

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No flights were found for this trip.", notFoundResult.Value);
        }

        [Fact]
        public void GetJourneyInternalServerError()
        {
            // Arrange
            var mockJourneyBusiness = _autoMoqer.GetMock<IJourneyBusiness>();

            var origin = "Origin";
            var destination = "Destination";
            var maxFlights = 2;

            mockJourneyBusiness
                .Setup(x => x.GetJourneys(origin, destination, maxFlights))
                .Throws(new Exception("Some error message"));

            // Act
            var result = _journeyController.GetJourney(origin, destination, maxFlights).Result;

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal Server Error", statusCodeResult.Value);
        }
    }
}
