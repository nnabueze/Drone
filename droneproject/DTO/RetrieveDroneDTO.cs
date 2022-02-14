using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DTO
{
    public class RetrieveDroneDTO
    {
        public string ReferenceKey { get; set; }

        public string SerialNumber { get; set; }

        public string Model { get; set; }


        public double Weight { get; set; }


        public double LoadingWeight { get; set; }


        public double Battery { get; set; }


        public string State { get; set; }

        
    }
}
