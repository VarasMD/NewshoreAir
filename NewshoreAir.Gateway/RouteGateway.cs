using NewshoreAir.Interface.Gateway;
using NewshoreApi.Entities.Entities;
using System.Text.Json;

namespace NewshoreAir.Gateway
{
    public class RouteGateway : IRouteGateway
    {
        public async Task<List<Route>> GetRoutes()
        {
            try
            {
                List<Route> routes = new List<Route>();
                var url = "https://recruiting-api.newshore.es/api/flights/2";
                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        routes = JsonSerializer.Deserialize<List<Route>>(content, options);
                    }
                }
                return routes;
            }
            catch
            {
                return new List<Route>();
            }
        }
    }
}
