using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Interface.Business
{
    public interface IJourneyBusiness
    {
        List<Journey> GetJourneys(string origin, string destination, int? maxFlights = null);
    }
}
