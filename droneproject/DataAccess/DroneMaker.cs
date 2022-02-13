using AutoMapper;
using droneproject.Domain;
using droneproject.Domain.Interface;
using droneproject.DTO;
using droneproject.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
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

        private IWebHostEnvironment _hostEnvironment;

        public DroneMaker(IGenericRepository<Drone> droneRepository, IMapper mapper, IGenericRepository<Mediation> mediationRepository, IWebHostEnvironment hostEnvironment)
        {
            _droneRepository = droneRepository;

            _mapper = mapper;

            _mediationRepository = mediationRepository;

            _hostEnvironment = hostEnvironment;
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
        public async Task<Response> LoadDrone(LoadDroneDTO request, IFormFile mediationImage)
        {
            //get a drone
            Drone drone = GetSingleDrone(request.DroneId);


            // check if the drone exist
            if (drone == null)

                return ResponseGenerator.CreateResponse("Invalid drone referencekey", 404, false);


            //check state of drone
            if (drone.State != StateStatus.IDLE || drone.State != StateStatus.LOADING)

                return ResponseGenerator.CreateResponse("Drone not in idle or loading state", 422, false);

            
            // check if the drone battery is low than 25%
            if (drone.Battery <= 25)

                return ResponseGenerator.CreateResponse("Failed to load drone battery is below 25%", 423, false);


            // check if single medication is heavyer than drone
            if(request.Mediations.Weight > drone.Weight)

                return ResponseGenerator.CreateResponse("Medication is heavy", 423, false);



            // total medication weight = loaded medication weight + loading medication weight
            var totalWeight = drone.LoadingWeight + request.Mediations.Weight;


            if (totalWeight <= drone.Weight)
            {
                //updating drone state before loading
                UpdateDroneState(request, StateStatus.LOADING, false);

                //Loading mediation
                await LoadDrone(request, mediationImage);

                //return response
                return ResponseGenerator.CreateResponse("Loading drone successful", 200, true);
            }
            else
            {
                //Updating drone after loading
                UpdateDroneState(request, StateStatus.LOADED, true);

                return ResponseGenerator.CreateResponse("Loading completed, failed loading", 423, false);
            }

        }


        /// <summary>
        /// Loading drone with mediations.
        /// </summary>
        /// <param name="request">Payload for loading drone</param>
        /// <param name="droneId">drone index id</param>
        public async void LoadingMediation(LoadDroneDTO request, int droneId, IFormFile mediationImage)
        {
            var imageReference = GetRandomInt(10);

            string imageId = UploadImage(imageReference, mediationImage);

            if (!string.IsNullOrEmpty(imageId))
            {
                var mediation = Mediation.Create(request.Mediations, imageId, droneId);

                var saveMediation = await _mediationRepository.Add(mediation);
            }

            await _mediationRepository.CommitAsync();
        }


        /// <summary>
        /// uploading mediation image
        /// </summary>
        /// <param name="imageReferenceKey">Unique image reference key</param>
        public string UploadImage(string imageReferenceKey, IFormFile mediationImage)
        {
            if (mediationImage.Length > 0)
            {
                string path = _hostEnvironment.WebRootPath + "\\Uploads\\";

                string fileName = imageReferenceKey + mediationImage.FileName.Trim();

                if (! Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = File.Create(path + fileName))
                {
                    mediationImage.CopyTo(fileStream);

                    fileStream.Flush();

                    return fileName;
                }
            }

            return "";
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
        /// Update drone
        /// </summary>
        /// <param name="request">Medication payload</param>
        /// <param name="status">drone status</param>
        /// <param name="isComplete">flag to check if drone loading or oaded</param>
        public async void UpdateDroneState(LoadDroneDTO request, StateStatus status, bool isComplete)
        {
            var drone = GetSingleDrone(request.DroneId);

            if (isComplete)

                drone.LoadingWeight = 0;

            switch (status)
            {
                case StateStatus.IDLE:
                    drone.State = StateStatus.IDLE;
                    break;
                case StateStatus.LOADING:
                    drone.State = StateStatus.LOADING;
                    break;
                case StateStatus.LOADED:
                    drone.State = StateStatus.LOADED;
                    break;
                case StateStatus.DELIVERING:
                    drone.State = StateStatus.DELIVERING;
                    break;
                case StateStatus.DELIVERED:
                    drone.State = StateStatus.DELIVERED;
                    break;
                case StateStatus.RETURNING:
                    drone.State = StateStatus.RETURNING;
                    break;
                default:
                    break;
            }
            

            _droneRepository.Update(drone);

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
