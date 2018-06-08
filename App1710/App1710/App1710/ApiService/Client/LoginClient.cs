using App1710.ApiHelper;
using App1710.ApiHelper.Client;
using App1710.ApiHelper.Response;
using App1710.ApiService.Model;
using App1710.ApiService.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1710.ApiService.Client
{
    public class LoginClient : ClientBase, ILoginClient
    {
        private const string RegisterUri = "api/register";
        private const string TokenUri = "api/token";

        public LoginClient(IApiClient iApiClient) : base(iApiClient)
        {
        }

        public async Task<TokenResponse> Login(string email, string password)
        {
            var response = await _iApiClient.PostFormEncodedContent(TokenUri, "grant_type".AsPair("password"),
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

        public async Task<RegisterResponse> Register(RegisterModel model)
        {
            var apiModel = new RegisterModel
            {
                ConfirmPassword = model.ConfirmPassword,
                Email = model.Email,
                Password = model.Password
            };
            var response = await _iApiClient.PostJsonEncodedContent(RegisterUri, apiModel);
            var registerResponse = await CreateJsonResponse<RegisterResponse>(response);
            return registerResponse;
        }
    }

    public interface ILoginClient
    {
        Task<TokenResponse> Login(string email, string password);
        Task<RegisterResponse> Register(RegisterModel model);
    }
}
