using AutoMapper;
using NewshoreAir.DTO.Flight.Response;
using NewshoreAir.DTO.Journey.Response;
using NewshoreAir.DTO.Transport.Response;
using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Journey, JourneyResponse>();
            CreateMap<Flight, FlightResponse>();
            CreateMap<Transport, TransportResponse>();
        }
    }
}
