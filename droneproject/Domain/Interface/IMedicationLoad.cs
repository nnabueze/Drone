using droneproject.DTO;
using droneproject.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Domain.Interface
{
    public interface IMedicationLoad
    {
        public Task<Response> Load(LoadDroneDTO request);

        public Response UploadImage(IFormFile mediationImage);
    }
}
