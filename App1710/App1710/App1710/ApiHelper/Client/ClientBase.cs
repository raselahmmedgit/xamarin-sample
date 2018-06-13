using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using App1710.ApiHelper.Response;
using App1710.ApiHelper.Model;
using Newtonsoft.Json;
using System;

namespace App1710.ApiHelper.Client
{
    public abstract class ClientBase
    {
        protected readonly IApiClient _iApiClient;

        protected ClientBase(IApiClient iApiClient)
        {
            _iApiClient = iApiClient;
        }

        protected async Task<TResponse> GetJsonListDecodedContent<TResponse, TContentResponse>(string uri, params KeyValuePair<string, string>[] requestParameters)
            where TResponse : ApiListResponse<TContentResponse>, new()
        {
            using (var apiResponse = await _iApiClient.GetFormEncodedContent(uri, requestParameters))
            {
                return await DecodeJsonListResponse<TResponse, TContentResponse>(apiResponse);
            }
        }

        protected async Task<TResponse> GetJsonDecodedContent<TResponse, TContentResponse>(string uri, params KeyValuePair<string, string>[] requestParameters)
            where TResponse : ApiResponse<TContentResponse>, new()
        {
            using (var apiResponse = await _iApiClient.GetFormEncodedContent(uri, requestParameters))
            {
                return await DecodeJsonResponse<TResponse, TContentResponse>(apiResponse);
            }
        }

        protected async Task<TResponse> PostEncodedContentWithSimpleResponse<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<int>, new()
        {
            using (var apiResponse = await _iApiClient.PostJsonEncodedContent(url, model))
            {
                return await DecodeJsonResponse<TResponse, int>(apiResponse);
            }
        }

        protected static async Task<TResponse> CreateJsonResponse<TResponse>(HttpResponseMessage response) where TResponse : ApiResponse, new()
        {
            var clientResponse = new TResponse
            {
                StatusIsSuccessful = response.IsSuccessStatusCode,
                ErrorState = response.IsSuccessStatusCode ? null : await DecodeContent<ErrorStateResponse>(response),
                ResponseCode = response.StatusCode
            };
            if (response.Content != null)
            {
                clientResponse.ResponseResult = await response.Content.ReadAsStringAsync();
            }

            return clientResponse;
        }

        protected static async Task<TContentResponse> DecodeContent<TContentResponse>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            var contentResponse = JsonConvert.DeserializeObject<TContentResponse>(result);
            return contentResponse;
        }

        private static async Task<TResponse> DecodeJsonListResponse<TResponse, TDecode>(HttpResponseMessage apiResponse) where TResponse : ApiListResponse<TDecode>, new()
        {
            var response = await CreateJsonResponse<TResponse>(apiResponse);

            try
            {
                var decodeData = JsonConvert.DeserializeObject<TDecode>(response.ResponseResult);
                response.DataList = decodeData;
            }
            catch (Exception)
            {
                response = new TResponse
                {
                    StatusIsSuccessful = false,
                    ErrorState = new ErrorStateResponse() { Message = "Unauthorized." },
                    ResponseCode = System.Net.HttpStatusCode.Unauthorized
                };
            }

            return response;
        }

        private static async Task<TResponse> DecodeJsonResponse<TResponse, TDecode>(HttpResponseMessage apiResponse) where TResponse : ApiResponse<TDecode>, new()
        {
            var response = await CreateJsonResponse<TResponse>(apiResponse);

            try
            {
                var decodeData = JsonConvert.DeserializeObject<TDecode>(response.ResponseResult);
                response.Data = decodeData;
            }
            catch (Exception)
            {
                response = new TResponse
                {
                    StatusIsSuccessful = false,
                    ErrorState = new ErrorStateResponse() { Message = "Unauthorized." },
                    ResponseCode = System.Net.HttpStatusCode.Unauthorized
                };
            }
            
            return response;
        }
    }
}
