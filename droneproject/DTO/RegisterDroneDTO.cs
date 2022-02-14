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

        [Required(ErrorMessage = "Battery level is required!")]
        [Range(0, 100, ErrorMessage = "Battery level is between 0 and 100 percent")]
        public double Battery { get; set; }

        [Required(ErrorMessage = "State is required!")]
        [EnumDataType(typeof(StateStatus), ErrorMessage = "State must be IDLE | LOADING | LOADED | DELIVERING | DELIVERED | RETURNING")]
        public string State { get; set; }
    }
}
