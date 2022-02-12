using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Domain
{
    public class Drone : BaseEntity
    {
        public string SerialNumber { get; set; }

        public string Model { get; set; }

        public int Weight { get; set; }

        public int Battery { get; set; }

        public string State { get; set; }
    }
}
