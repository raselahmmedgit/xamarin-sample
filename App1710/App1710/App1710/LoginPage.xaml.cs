using App1710.ApiHelper;
using App1710.ApiHelper.Client;
using App1710.ApiService.Client;
using App1710.ApiService.Model;
using App1710.AppCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1710
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private readonly ILoginClient _iLoginClient;
        private readonly ITokenContainer _iTokenContainer;

        public LoginPage ()
		{
			InitializeComponent ();

            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iLoginClient = new LoginClient(apiClient);
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var userModel = new UserModel
            {
                Email = emailEntry.Text,
                Password = passwordEntry.Text
            };

            var appMessage = await LoginUserAsync(userModel);
            if (appMessage.IsSuccess)
            {
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = appMessage.Message;
                passwordEntry.Text = string.Empty;
            }
        }

        async Task<AppMessage> LoginUserAsync(UserModel userModel)
        {
            AppMessage appMessage = IsValid(userModel);

            #region Login

            if (appMessage.IsSuccess)
            {
                #region RestClient

                try
                {
                    var isLogin = await IsLoginUserAsync(userModel.Email, userModel.Password);
                    appMessage = appMessage.SetSuccess("Sign up successfully.");
                }
                catch (Exception ex)
                {
                    appMessage = appMessage.SetError("Sign up failed.");
                }

                #endregion

            }

            #endregion

            return appMessage;
        }

        private async Task<bool> IsLoginUserAsync(string email, string password)
        {
            var response = await _iLoginClient.Login(email, password);
            if (response.StatusIsSuccessful)
            {
                _iTokenContainer.ApiToken = response.Data;
            }

            return response.StatusIsSuccessful;
        }

        AppMessage IsValid(UserModel userModel)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(userModel.Email))
            {
                return appMessage = appMessage.SetError("Email is required.");
            }
            if (!userModel.Email.Contains("@"))
            {
                return appMessage = appMessage.SetError("Email is not valid.");
            }
            if (string.IsNullOrWhiteSpace(userModel.Password))
            {
                return appMessage = appMessage.SetError("Password is required.");
            }

            return appMessage = appMessage.SetSuccess();
        }
    }
}