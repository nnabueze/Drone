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

namespace droneproject.DataAccess
{
    public class MedicationLoad : IMedicationLoad
    {
        private readonly IGenericRepository<Drone> _droneRepository;

        private readonly IGenericRepository<Mediation> _mediationRepository;

        private readonly IMapper _mapper;

        private readonly IHostingEnvironment _hostingEnvironment;

        public MedicationLoad(IGenericRepository<Drone> droneRepository, IGenericRepository<Mediation> mediationRepository,

            IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _droneRepository = droneRepository;

            _mediationRepository = mediationRepository;

            _mapper = mapper;

            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// Loading medication
        /// </summary>
        /// <param name="request"></param>
        /// <param name="mediationImage"></param>
        /// <returns></returns>
        public async Task<Response> Load(LoadDroneDTO request)
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
            if (request.Mediations.Weight > drone.Weight)

                return ResponseGenerator.CreateResponse("Medication is heavy", 423, false);



            // total medication weight = loaded medication weight + loading medication weight
            var totalWeight = drone.LoadingWeight + request.Mediations.Weight;


            if (totalWeight <= drone.Weight)
            {
                //updating drone state before loading
                UpdateDroneState(request, StateStatus.LOADING, false);

                //Loading mediation
                var isLoaded = await LoadingMediation(request, drone.Id);

                if(isLoaded)

                    return ResponseGenerator.CreateResponse("Loading drone successful", 200, true);


                return ResponseGenerator.CreateResponse("failed loading drone", 423, false);
            }
            else
            {
                //Updating drone after loading
                UpdateDroneState(request, StateStatus.LOADED, true);

                return ResponseGenerator.CreateResponse("Loading completed, failed loading", 423, false);
            }
        }



        /// <summary>
        /// Loadig medication
        /// </summary>
        /// <param name="request"></param>
        /// <param name="droneId"></param>
        /// <param name="mediationImage"></param>
        /// <returns></returns>
        public async Task<bool> LoadingMediation(LoadDroneDTO request, int droneId)
        {

            var mediation = Mediation.Create(request.Mediations, droneId);

            var saveMediation = await _mediationRepository.Add(mediation);

            await _mediationRepository.CommitAsync();

            if (saveMediation != null)
            {
                return true;
            }

            return false;
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
        /// Retrieving a single drone
        /// </summary>
        /// <returns></returns>
        public Drone GetSingleDrone(string referenceKey)
        {
            return _droneRepository.FindFirst(x => x.ReferenceKey == referenceKey);
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


        /// <summary>
        /// uploading medication image
        /// </summary>
        /// <param name="mediationImage"></param>
        /// <returns></returns>
        public Response UploadImage(IFormFile mediationImage)
        {
            var response = Upload(mediationImage);

            if(string.IsNullOrEmpty(response))

                return ResponseGenerator.CreateResponse("failed uploading image", 423, false);


            return ResponseGenerator.CreateResponse("Image uploaded successfully", 200, true, new { imageReference = response});
        }




        /// <summary>
        /// uploading mediation image
        /// </summary>
        /// <param name="imageReferenceKey">Unique image reference key</param>
        public string Upload(IFormFile mediationImage)
        {
            var imageReferenceKey = GetRandomInt(5);

            if (mediationImage.Length > 0)
            {
                string projectRootPath = _hostingEnvironment.ContentRootPath;

                string path = projectRootPath + "\\Uploads\\";

                string fileName = imageReferenceKey + mediationImage.FileName.Trim();

                var fullPath = Path.Combine(path, fileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    mediationImage.CopyTo(stream);

                    stream.Flush();

                    return fileName;
                }
            }

            return "";
        }
    }
}
