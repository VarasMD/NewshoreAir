using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewshoreAir.DTO.Journey.Response;
using NewshoreAir.Interface.Business;

namespace NewshoreAir.Controllers
{
    public class JourneyController : ControllerBase
    {
        #region Private Field
        private readonly IMapper _mapper;
        private readonly IJourneyBusiness _journeyBusiness;
        #endregion

        #region Constructor
        public JourneyController(IMapper mapper, IJourneyBusiness journeyBusiness)
        {
            _mapper = mapper;
            _journeyBusiness = journeyBusiness;
        }
        #endregion

        #region Public Methods
        [HttpGet("{origin}/{destination}")]
        public ActionResult<List<JourneyResponse>> GetJourney([FromQuery] string origin, [FromQuery] string destination, [FromQuery] int? maxFlights)
        {
            var journeys = _journeyBusiness.GetJourneys(origin, destination, maxFlights);

            var journeysResponse = _mapper.Map<JourneyResponse>(journeys);

            return Ok(journeysResponse);
        }
        #endregion
    }
}
