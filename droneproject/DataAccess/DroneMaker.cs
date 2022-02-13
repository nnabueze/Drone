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
    public class DroneMaker : IDroneMaker
    {
        private readonly IGenericRepository<Drone> _droneRepository;

        private readonly IMapper _mapper;

        public DroneMaker(IGenericRepository<Drone> droneRepository, IMapper mapper)
        {
            _droneRepository = droneRepository;

            _mapper = mapper;
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

        public Task<Response> CreateDrone(LoadDroneDTO request)
        {
            throw new NotImplementedException();
        }


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
