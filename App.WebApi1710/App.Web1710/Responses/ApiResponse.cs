using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace App.Web1710.Responses
{
    public abstract class ApiResponse
    {
        public bool StatusIsSuccessful { get; set; }
        public ErrorStateResponse ErrorState { get; set; }
        public HttpStatusCode ResponseCode { get; set; }
        public string ResponseResult { get; set; }
    }

    public abstract class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}