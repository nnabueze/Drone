using AutoMapper;
using droneproject.Domain;
using droneproject.Domain.Interface;
using droneproject.DTO;
using droneproject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DataAccess
{
    public class AvilableDrone : IAvilableDrone
    {
        private readonly IGenericRepository<Drone> _droneRepository;

        private readonly IGenericRepository<Mediation> _mediationRepository;

        private readonly IMapper _mapper;

        public AvilableDrone(IGenericRepository<Drone> droneRepository, IGenericRepository<Mediation> mediationRepository, IMapper mapper)
        {
            _droneRepository = droneRepository;

            _mediationRepository = mediationRepository;

            _mapper = mapper;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response RetrieveDrone()
        {
            List<RetrieveDroneDTO> availDrone = new List<RetrieveDroneDTO>();

            //asuming number of drone is 10
            var drones = _droneRepository.FindAll();

            foreach (var x in drones)
            {
                var droneState = GetDroneState(x.State);

                var modelState = GetDroneModelState(x.Model);

                //returning only idle and loading drone that is availabl for loading
                if (droneState.Equals("IDLE") || droneState.Equals("LOADING"))
                {
                    var drone = new RetrieveDroneDTO()
                    {
                        ReferenceKey = x.ReferenceKey,

                        State = droneState,

                        Battery = x.Battery,

                        LoadingWeight = x.LoadingWeight,

                        SerialNumber = x.SerialNumber,

                        Weight = x.Weight,

                        Model = modelState

                    };

                    availDrone.Add(drone);
                }

                    
            }

            return ResponseGenerator.CreateResponse("sucessful", 200, true, availDrone);
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


        public string GetDroneModelState(ModelStatus modelStatus)
        {
            switch (modelStatus)
            {
                case ModelStatus.Lightweight:
                    return "Lightweight";

                case ModelStatus.Middleweight:
                    return "Middleweight";

                case ModelStatus.Cruiserweight:
                    return "Cruiserweight";

                default:
                    return "Heavyweight";
            }
        }

        public Response CheckBatteryLevel(string dronId)
        {
            var drone = GetSingleDrone(dronId);

            if(drone == null)

                return ResponseGenerator.CreateResponse("Drone not found", 404,false);

            return ResponseGenerator.CreateResponse("Sucessful", 200, true, new { droneId = drone.ReferenceKey, batteryLevel = drone.Battery });
        }


        /// <summary>
        /// Retrieving a single drone
        /// </summary>
        /// <returns></returns>
        public Drone GetSingleDrone(string referenceKey)
        {
            return _droneRepository.FindFirst(x => x.ReferenceKey == referenceKey);
        }
    }
}
