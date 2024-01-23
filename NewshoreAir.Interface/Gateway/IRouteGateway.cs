using NewshoreApi.Entities.Entities;

namespace NewshoreAir.Interface.Gateway
{
    public interface IRouteGateway
    {
        Task<List<Route>> GetRoutes();
    }
}
