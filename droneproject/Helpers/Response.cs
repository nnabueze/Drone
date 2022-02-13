﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Helpers
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public object Data { get; set; }
    }
}
