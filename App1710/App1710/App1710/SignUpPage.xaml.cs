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
	public partial class SignUpPage : ContentPage
	{
        private readonly ILoginClient _iLoginClient;
        private readonly ITokenContainer _iTokenContainer;

        public SignUpPage ()
		{
			InitializeComponent ();
            _iTokenContainer = new TokenContainer();
            if (_iTokenContainer.IsApiCurrentToken())
            {
                Navigation.InsertPageBefore(new LoginPage(), this);
                Navigation.PopAsync();
            }
            
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iLoginClient = new LoginClient(apiClient);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            _iTokenContainer.ClearApiCurrentToken();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }


        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            _iTokenContainer.ClearApiCurrentToken();

            var registerModel = new RegisterModel()
            {
                Email = emailEntry.Text,
                Password = passwordEntry.Text,
                ConfirmPassword = confirmpasswordEntry.Text
            };

            // Sign up logic goes here

            var appMessage = await SignUpAsync(registerModel);

            if (appMessage.IsSuccess)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    Navigation.InsertPageBefore(new LoginPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = appMessage.Message;
            }
        }

        async Task<AppMessage> SignUpAsync(RegisterModel registerModel)
        {
            AppMessage appMessage = IsValid(registerModel);

            if (appMessage.IsSuccess)
            {
                #region Api Call

                RegisterModel model = new RegisterModel
                {
                    ConfirmPassword = registerModel.ConfirmPassword,
                    Password = registerModel.Password,
                    Email = registerModel.Email
                };

                try
                {
                    var responseStatus = await CreateUserAsync(model);
                    appMessage = appMessage.SetSuccess("Sign up successfully.");
                }
                catch (Exception ex)
                {
                    appMessage = appMessage.SetError("Sign up failed.");
                }

                #endregion
            }

            return appMessage ;
        }

        private async Task<bool> CreateUserAsync(RegisterModel model)
        {
            var response = await _iLoginClient.Register(model);
            return response.StatusIsSuccessful;
        }

        AppMessage IsValid(RegisterModel registerModel)
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrWhiteSpace(registerModel.Email))
            {
                return appMessage = appMessage.SetError("Email is required.");
            }
            if (!registerModel.Email.Contains("@"))
            {
                return appMessage = appMessage.SetError("Email is not valid.");
            }
            if (string.IsNullOrWhiteSpace(registerModel.Password))
            {
                return appMessage = appMessage.SetError("Password is required.");
            }
            if (string.IsNullOrWhiteSpace(registerModel.ConfirmPassword))
            {
                return appMessage = appMessage.SetError("Confirm password is required.");
            }
            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                return appMessage = appMessage.SetError("The new password and confirmation password do not match..");
            }

            return appMessage = appMessage.SetSuccess();
        }
    }
}