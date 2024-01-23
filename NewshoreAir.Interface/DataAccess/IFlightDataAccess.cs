using NewshoreApi.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewshoreAir.Interface.DataAccess
{
    public interface IFlightDataAccess
    {
        public List<Flight> GetFlights();
    }
}
