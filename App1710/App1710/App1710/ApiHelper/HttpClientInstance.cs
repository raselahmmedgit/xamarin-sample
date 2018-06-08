using App1710.AppCore;
using System;
using System.Net.Http;

namespace App1710.ApiHelper
{
    public static class HttpClientInstance
    {
        private const string BaseUri = AppConstant.BaseAddress;
        private static readonly HttpClient instance = new HttpClient { BaseAddress = new Uri(BaseUri) };

        public static HttpClient Instance
        {
            get { return instance; }
        }
    }
}
