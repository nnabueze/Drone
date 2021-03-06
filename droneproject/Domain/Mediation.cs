using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static droneproject.DTO.LoadDroneDTO;

namespace droneproject.Domain
{
    public class Mediation : BaseEntity
    {
        public string Name { get; set; }

        public double Weight { get; set; }

        public string Code { get; set; }

        public string ImageId { get; set; }

        public int DroneId { get; set; }

        public Drone Drone { get; set; }

        public Mediation()
        {

        }

        public Mediation(Item request, int droneId)
        {
            Name = request.Name;

            Weight = request.Weight;

            Code = request.Code;

            ImageId = request.ImageReference;

            DroneId = droneId;

        }

        public static Mediation Create(Item request, int droneId)
        {
            return new Mediation(request, droneId);
        } 
    }
}
