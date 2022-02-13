using droneproject.DTO;
using droneproject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Domain.Interface
{
    public interface IDroneMaker
    {
        public Task<Response> CreateDrone(RegisterDroneDTO request);

        public Task<Response> CreateDrone(LoadDroneDTO request);
    }
}
