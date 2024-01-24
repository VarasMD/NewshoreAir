using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Interface.DataAccess
{
    public interface IJourneyDataAccess
    {
        public List<Journey> GetJourneys(string origin, string destination);
        public void SaveJourney(Journey journey);
    }
}
