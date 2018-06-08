using App.Web1710.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace App.Web1710.Models
{
    public interface ILoginClient
    {
        Task<RegisterResponse> Register(RegisterViewModel viewModel);
    }

    public class LoginClient : ClientBase, ILoginClient
    {
        private const string TokenUri = "api/token";
        private const string RegisterUri = "api/register";

        public LoginClient(IApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<TokenResponse> Login(string email, string password)
        {
            var response = await ApiClient.PostFormEncodedContent(TokenUri, "grant_type".AsPair("password"),
                "username".AsPair(email), "password".AsPair(password));
            var tokenResponse = await CreateJsonResponse<TokenResponse>(response);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await DecodeContent<dynamic>(response);
                tokenResponse.ErrorState = new ErrorStateResponse
                {
                    ModelState = new Dictionary<string, string[]>
                {
                    {errorContent["error"], new string[] {errorContent["error_description"]}}
                }
                };
                return tokenResponse;
            }

            var tokenData = await DecodeContent<dynamic>(response);
            tokenResponse.Data = tokenData["access_token"];
            return tokenResponse;
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

        public async Task<RegisterResponse> Register(RegisterViewModel viewModel)
        {
            var apiModel = new RegisterViewModel
            {
                ConfirmPassword = viewModel.ConfirmPassword,
                Email = viewModel.Email,
                Password = viewModel.Password
            };
            var response = await ApiClient.PostJsonEncodedContent(RegisterUri, apiModel);
            var registerResponse = await CreateJsonResponse<RegisterResponse>(response);
            return registerResponse;
        }

        private static async Task<TContentResponse> DecodeContent<TContentResponse>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Json.Decode<TContentResponse>(result);
        }
    }
}