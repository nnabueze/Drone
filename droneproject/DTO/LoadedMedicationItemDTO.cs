using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DTO
{
    public class LoadedMedicationItemDTO
    {
        public string DroneId { get; set; }

        public string State { get; set; }

        public List<Item> Medications { get; set; }
        public class Item
        {
            public string Name { get; set; }

            public double Weight { get; set; }

            public string Code { get; set; }

            public string ImageId { get; set; }

        }
    }
}
