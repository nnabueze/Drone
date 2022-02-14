using droneproject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Domain.Interface
{
    public interface IAvilableDrone
    {
        public Response RetrieveDrone();

        public Response CheckBatteryLevel(string dronId);
    }
}
