using AutoMapper;
using droneproject.Domain;
using droneproject.Domain.Interface;
using droneproject.DTO;
using droneproject.Helpers;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static droneproject.DTO.LoadedMedicationItemDTO;

namespace droneproject.DataAccess
{
    public class MedicationLoadedItem : IMedicationLoadedItem
    {
        private readonly IGenericRepository<Drone> _droneRepository;

        private readonly IGenericRepository<Mediation> _mediationRepository;

        private readonly IMapper _mapper;

        public MedicationLoadedItem(IGenericRepository<Drone> droneRepository, IGenericRepository<Mediation> mediationRepository, IMapper mapper)
        {
            _droneRepository = droneRepository;

            _mediationRepository = mediationRepository;

            _mapper = mapper;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public async Task<Response> LoadedMedicationItem(string droneId)
        {
            List<Item> arrMedications = new List<Item>();

            // reterive drone by id
            var drone = GetSingleDrone(droneId);

            if(drone == null)

                return ResponseGenerator.CreateResponse("drone Not found", 404, false);

            //reterive drone medication
            var arr = await _mediationRepository.FindBy(x => x.DroneId == drone.Id);

            foreach (var x in arr)
            {
                var singleItem = new Item()
                {
                    Code = x.Code,

                    ImageId = x.ImageId,

                    Name = x.Name,

                    Weight = x.Weight
                };

                arrMedications.Add(singleItem);
            }

            var droneState = GetDroneState(drone.State);

            var response = new LoadedMedicationItemDTO()
            {
                DroneId = drone.ReferenceKey,

                State = droneState,

                Medications = arrMedications
            };

            //return response
            return ResponseGenerator.CreateResponse("sucessful", 200, true, response);
        }


        /// <summary>
        /// Retrieving a single drone
        /// </summary>
        /// <returns></returns>
        public Drone GetSingleDrone(string referenceKey)
        {
            return _droneRepository.FindFirst(x => x.ReferenceKey == referenceKey);
        }

        public string GetDroneState(StateStatus stateStatus)
        {
            switch (stateStatus)
            {
                case StateStatus.IDLE:
                    return "IDLE";
                    
                case StateStatus.LOADING:
                    return "LOADING";
                    
                case StateStatus.LOADED:
                    return "LOADED";
                    
                case StateStatus.DELIVERING:
                    return "DELIVERING";
                    
                case StateStatus.DELIVERED:
                    return "DELIVERED";
                                       
                default:
                    return "RETURNING";
            }
        }
    }
}
