using AutoMapper;
using droneproject.Domain;
using droneproject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.AutoMapper
{
    public class DroneProfile : Profile
    {
        public DroneProfile()
        {
            CreateMap<RegisterDroneDTO, Drone>().ReverseMap();
        }
    }
}
