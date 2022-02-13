using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DTO
{
    public class ReturnRegDroneDTO
    {
        public string droneId { get; set; }

        public ReturnRegDroneDTO()
        {

        }

        public ReturnRegDroneDTO(string droneId)
        {
            droneId = droneId;
        }

        public static ReturnRegDroneDTO Create(string droneId)
        {
            return new ReturnRegDroneDTO(droneId);
        }
    }
}
