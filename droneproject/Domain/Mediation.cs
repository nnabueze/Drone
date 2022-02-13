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

        public Mediation(Item request, string imageReferenceKey, int droneId)
        {
            Name = request.Name;

            Weight = request.Weight;

            Code = request.Code;

            ImageId = imageReferenceKey;

            DroneId = droneId;

        }

        public static Mediation Create(Item request, string imageReferenceKey, int droneId)
        {
            return new Mediation(request, imageReferenceKey, droneId);
        } 
    }
}
