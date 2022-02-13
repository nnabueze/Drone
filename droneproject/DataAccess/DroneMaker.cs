using AutoMapper;
using droneproject.Domain;
using droneproject.Domain.Interface;
using droneproject.DTO;
using droneproject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static droneproject.DTO.LoadDroneDTO;

namespace droneproject.DataAccess
{
    public class DroneMaker : IDroneMaker
    {
        private readonly IGenericRepository<Drone> _droneRepository;

        private readonly IGenericRepository<Mediation> _mediationRepository;

        private readonly IMapper _mapper;

        public DroneMaker(IGenericRepository<Drone> droneRepository, IMapper mapper, IGenericRepository<Mediation> mediationRepository)
        {
            _droneRepository = droneRepository;

            _mapper = mapper;

            _mediationRepository = mediationRepository;
        }

        public async Task<Response> CreateDrone(RegisterDroneDTO request)
        {
            //add auto mapper
            var mappedrone = _mapper.Map<RegisterDroneDTO, Drone>(request);

            //generate random key reference
            var referenceKey = GetRandomInt(10);

            mappedrone.ReferenceKey = referenceKey;

            //save or create drone
            var savedDrone = await _droneRepository.Add(mappedrone);

            await _droneRepository.CommitAsync();

            //return response
            return ResponseGenerator.CreateResponse("Successfully registered drone", 201, true, new {  droneId = savedDrone.ReferenceKey });
        }


        /// <summary>
        /// Loading drone
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response> LoadDrone(LoadDroneDTO request)
        {
            //check if drone id exist
            Drone drone = GetSingleDrone(request.DroneId);

            if (drone == null)

                return ResponseGenerator.CreateResponse("Invalid drone referencekey", 404, false);

            //get the total weight of mediiation

            var totalWeight = request.Mediations.Sum(x => x.Weight);

            if (drone.Battery <= 25)

                return ResponseGenerator.CreateResponse("Failed to load drone battery below 25%", 423, false);

            if(drone.Weight < totalWeight)

                return ResponseGenerator.CreateResponse("Mediations heavy than drone", 423, false);

            if (drone.State != StateStatus.IDLE)

                return ResponseGenerator.CreateResponse("Drone not in idle state", 422, false);

            //updating drone state before loading
            UpdateDroneState(drone, StateStatus.LOADING);

            //Loading mediation
            await LoadDrone(request);

            //Updating drone after loading
            UpdateDroneState(drone, StateStatus.LOADED);

            //return response
            return ResponseGenerator.CreateResponse("Drone loaded", 200, true);
        }


        /// <summary>
        /// Loading drone with mediations.
        /// </summary>
        /// <param name="request">Payload for loading drone</param>
        /// <param name="droneId">drone index id</param>
        public async void LoadingMediation(LoadDroneDTO request, int droneId)
        {
            foreach (var item in request.Mediations)
            {
                var imageReference = GetRandomInt(10);

                var mediation = Mediation.Create(item, imageReference, droneId);

                var saveMediation = await _mediationRepository.Add(mediation);

                if (saveMediation != null)

                    UploadImage(saveMediation.ImageId);
            }            

            await _mediationRepository.CommitAsync();
        }


        /// <summary>
        /// uploading mediation image
        /// </summary>
        /// <param name="imageReferenceKey">Unique image reference key</param>
        public void UploadImage(string imageReferenceKey)
        {

        }

        /// <summary>
        /// Retrieving a single drone
        /// </summary>
        /// <returns></returns>
        public Drone GetSingleDrone(string referenceKey)
        {
            return _droneRepository.FindFirst(x => x.ReferenceKey == referenceKey);
        }

        /// <summary>
        /// update the state of drone
        /// </summary>
        /// <param name="request"></param>
        public async void UpdateDroneState(Drone request, StateStatus status)
        {
            switch (status)
            {
                case StateStatus.IDLE:
                    request.State = StateStatus.IDLE;
                    break;
                case StateStatus.LOADING:
                    request.State = StateStatus.LOADING;
                    break;
                case StateStatus.LOADED:
                    request.State = StateStatus.LOADED;
                    break;
                case StateStatus.DELIVERING:
                    request.State = StateStatus.DELIVERING;
                    break;
                case StateStatus.DELIVERED:
                    request.State = StateStatus.DELIVERED;
                    break;
                case StateStatus.RETURNING:
                    request.State = StateStatus.RETURNING;
                    break;
                default:
                    break;
            }
            

            _droneRepository.Update(request);

            await _droneRepository.CommitAsync();
        }


        /// <summary>
        /// Generate randow numbers
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetRandomInt(int length)
        {
            var rnd = new Random(DateTime.UtcNow.Millisecond);

            string rNum = DateTime.UtcNow.Millisecond + rnd.Next(0, 900000000).ToString();

            string temp = "";

            for (int i = 0; i < length; i++)
            {
                temp += rNum[rnd.Next(0, rNum.Length)].ToString();
            }

            return temp;
        }
    }
}
