using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DTO
{
    public class LoadDroneDTO
    {
        [Required(ErrorMessage = "DroneId is required!")]
        public string DroneId { get; set; }

        public Item Mediations { get; set; }

        public class Item
        {
            [Required(ErrorMessage = "Weight is required!")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Weight is required!")]
            public double Weight { get; set; }

            [Required(ErrorMessage = "Code is required!")]
            public string Code { get; set; }

            public string ImageReference { get; set; }
        }
    }
}
