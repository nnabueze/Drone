﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
