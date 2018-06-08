using App.Web1710.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace App.Web1710.Models
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient httpClient;
        private const string BaseUri = "http://localhost:14180/";

        public ApiClient()
        {
            this.httpClient = new HttpClient();
        }


        

        //public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.BaseAddress = new Uri(BaseUri);
        //        var response = await httpClient.GetAsync(requestUri);
        //        return response;
        //    }
        //}

        public async Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel
        {
            httpClient.BaseAddress = new Uri(BaseUri);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsJsonAsync(requestUri, content);
            return response;
        }

        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            httpClient.BaseAddress = new Uri(BaseUri);
            using (var content = new FormUrlEncodedContent(values))
            {
                var query = await content.ReadAsStringAsync();
                var requestUriWithQuery = string.Concat(requestUri, "?", query);
                var response = await httpClient.GetAsync(requestUriWithQuery);
                return response;
            }
        }
    }
}