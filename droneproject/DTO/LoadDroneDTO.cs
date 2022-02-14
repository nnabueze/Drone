using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DTO
{
    public class LoadDroneDTO
    {
        public string DroneId { get; set; }

        public Item Mediations { get; set; }

        public class Item
        {
            public string Name { get; set; }

            public double Weight { get; set; }

            public string Code { get; set; }

            public string ImageReference { get; set; }
        }
    }
}
