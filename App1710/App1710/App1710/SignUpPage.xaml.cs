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
		public SignUpPage ()
		{
			InitializeComponent ();
		}

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }


        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            var registerModel = new RegisterModel()
            {
                Email = emailEntry.Text,
                Password = passwordEntry.Text,
                ConfirmPassword = confirmpasswordEntry.Text
            };

            // Sign up logic goes here

            var appMessage = await CreateUserAsync(registerModel);

            if (appMessage.IsSuccess)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                messageLabel.Text = appMessage.Message;
            }
        }

        async Task<AppMessage> CreateUserAsync(RegisterModel registerModel)
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

                #endregion
            }

            return appMessage ;
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