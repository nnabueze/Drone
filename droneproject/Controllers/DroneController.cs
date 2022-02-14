using droneproject.Domain.Interface;
using droneproject.DTO;
using droneproject.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneController : ControllerBase
    {

        private readonly IDroneMaker _droneMaker;

        private readonly IMedicationLoad _medicationLoad;

        public DroneController(IDroneMaker droneMaker, IMedicationLoad medicationLoad)
        {
            _droneMaker = droneMaker;

            _medicationLoad = medicationLoad;
        }

        /// <summary>
        /// API for registering a drone
        /// </summary>
        [HttpPost]
        [Route("RegisterDrone")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] RegisterDroneDTO request)
        {

            try
            {
                var result = await _droneMaker.CreateDrone(request);

                var response = new JsonResult(result);

                response.StatusCode = result.StatusCode;

                return response;
            }
            catch (Exception ex)
            {

                var response = new JsonResult(ResponseGenerator.CreateResponse(ex.Message.ToString(), 500, false));

                response.StatusCode = 500;

                return response;

            }
        }



        /// <summary>
        /// Upload medication image
        /// </summary>
        /// <param name="mediationImage"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImageUpload")]
        public IActionResult Upload(IFormFile mediationImage)
        {
            try
            {
                var result = _medicationLoad.UploadImage(mediationImage);

                var response = new JsonResult(result);

                response.StatusCode = result.StatusCode;

                return response;
            }
            catch (Exception ex)
            {

                var response = new JsonResult(ResponseGenerator.CreateResponse(ex.Message.ToString(), 500, false));

                response.StatusCode = 500;

                return response;

            }
        }


        /// <summary>
        /// API for loading a drone with medication items
        /// </summary>
        [HttpPost]
        [Route("LoadDrone")]
        public async Task<IActionResult> Load([FromBody] LoadDroneDTO request )
        {
            try
            {
                var result = await _medicationLoad.Load(request);

                var response = new JsonResult(result);

                response.StatusCode = result.StatusCode;

                return response;
            }
            catch (Exception ex)
            {

                var response = new JsonResult(ResponseGenerator.CreateResponse(ex.Message.ToString(), 500, false));

                response.StatusCode = 500;

                return response;

            }
        }       



        /// <summary>
        /// API for checking loaded medication items for a given drone
        /// </summary>
        [HttpGet]
        [Route("LoadedDroneItem")]
        public IActionResult MedicationItem()
        {
            return Ok();
        }


        /// <summary>
        /// API for checking available drones for loading
        /// </summary>
        [HttpGet]
        [Route("AvailableDones")]
        public IActionResult AvailableDones()
        {
            return Ok();
        }


        /// <summary>
        /// API for check drone battery level for a given drone
        /// </summary>
        [HttpGet]
        [Route("CheckBatteryLevel")]
        public IActionResult BatteryLevel()
        {
            return Ok();
        }
    }
}
