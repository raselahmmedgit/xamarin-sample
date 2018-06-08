using System.Collections.Generic;

namespace App1710.ApiHelper.Response
{
    public class ErrorStateResponse
    {
        public string Message { get; set; }
        public IDictionary<string, string[]> ModelState { get; set; }
    }
}
