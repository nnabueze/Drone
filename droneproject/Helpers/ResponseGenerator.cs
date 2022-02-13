using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Helpers
{
    public class ResponseGenerator
    {
        public static Response CreateResponse(string msg, int code, bool status, object data = null)
        {
            return new Response()
            {
                Message = msg,

                StatusCode = code,

                IsSuccess = status,

                Data = data
            };
        }
    }
}
