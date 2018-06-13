using System.Collections.Generic;
using System.Net;

namespace App1710.ApiHelper.Response
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

    public abstract class ApiListResponse<T> : ApiResponse
    {
        public T DataList { get; set; }
    }
}
