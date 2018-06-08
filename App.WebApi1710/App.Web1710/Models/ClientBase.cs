using App.Web1710.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace App.Web1710.Models
{

    public abstract class ClientBase
    {
        private readonly IApiClient apiClient;

        protected ClientBase(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        protected async Task<TResponse> PostEncodedContentWithSimpleResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await apiClient.PostJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, int>(apiResponse);
            }
        }

        protected async Task<TResponse> GetJsonDecodedContent<TResponse, TContentResponse>(string uri, params KeyValuePair<string, string>[] requestParameters) where TResponse : ApiResponse<TContentResponse>, new()
        {
            var apiResponse = await apiClient.GetFormEncodedContent(uri, requestParameters);
            var response = await CreateJsonResponse<TResponse>(apiResponse);
            response.Data = JsonConvert.DeserializeObject<TContentResponse>(response.ResponseResult);
            return response;
        }

        private static async Task<TResponse> CreateJsonResponse<TResponse>(HttpResponseMessage response) where TResponse : ApiResponse, new()
        {
            var clientResponse = new TResponse
            {
                StatusIsSuccessful = response.IsSuccessStatusCode,
                ResponseCode = response.StatusCode
            };
            if (response.Content != null)
            {
                clientResponse.ResponseResult = await response.Content.ReadAsStringAsync();
            }

            return clientResponse;
        }
    }
}