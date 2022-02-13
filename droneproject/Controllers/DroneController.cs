using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Create drone
        /// </summary>
        [HttpPost]
        [Route("CreateDrone")]
        public IActionResult Create()
        {
            return Ok();
        }
    }
}
