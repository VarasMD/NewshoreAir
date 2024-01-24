using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewshoreAir.DTO.Journey.Response;
using NewshoreAir.Interface.Business;

namespace NewshoreAir.Controllers
{
    [Route("api/journeys")]
    public class JourneyController : ControllerBase
    {
        #region Private Field
        private readonly IMapper _mapper;
        private readonly IJourneyBusiness _journeyBusiness;
        private readonly ILogger<JourneyController> _logger;
        #endregion

        #region Constructor
        public JourneyController(IMapper mapper, IJourneyBusiness journeyBusiness, ILogger<JourneyController> logger)
        {
            _mapper = mapper;
            _journeyBusiness = journeyBusiness;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        [HttpGet("{origin}/{destination}")]
        public ActionResult<List<JourneyResponse>> GetJourney(string origin, string destination, [FromQuery] int? maxFlights)
        {
            _logger.LogInformation("A request was received at Endpoint");
            var journeys = _journeyBusiness.GetJourneys(origin, destination, maxFlights);

            var journeysResponse = _mapper.Map<List<JourneyResponse>>(journeys);
            _logger.LogInformation("Successfully generated response in Endpoint");

            return Ok(journeysResponse);
        }
        #endregion
    }
}
