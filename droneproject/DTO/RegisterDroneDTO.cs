using droneproject.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DTO
{
    public class RegisterDroneDTO
    {
        [Required(ErrorMessage = "SerialNumber is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Maximum 100 characters and minimun of 3 characters")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Model is required!")]
        [EnumDataType(typeof(ModelStatus), ErrorMessage = "Model must be Lightweight | Middleweight | Cruiserweight | Heavyweight")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Weight is required!")]
        [Range(0, 500, ErrorMessage = "Weight is between 1 and 500")]
        public double Weight { get; set; }
    }
}
