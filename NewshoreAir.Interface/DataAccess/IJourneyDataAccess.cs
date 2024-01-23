using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Interface.DataAccess
{
    public interface IJourneyDataAccess
    {
        public List<Journey> GetJourneys();
        public void SaveJourney(Journey journey);
    }
}
