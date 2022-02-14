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
