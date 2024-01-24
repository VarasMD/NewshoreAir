using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Interface.DataAccess
{
    public interface IJourneyDataAccess
    {
        public List<Journey> GetJourneys(string origin, string destination, int? maxFlights = null);
        public void SaveJourney(List<Journey> journey);
    }
}
