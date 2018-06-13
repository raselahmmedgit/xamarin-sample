using App1710.ApiHelper;
using App1710.ApiHelper.Client;
using App1710.ApiService.Client;
using App1710.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1710
{
	public partial class MainPage : ContentPage
	{
        private readonly IStudentClient _iStudentClient;
        private readonly ITokenContainer _iTokenContainer;

        public MainPage()
		{
			InitializeComponent();
            _iTokenContainer = new TokenContainer();
            if (!_iTokenContainer.IsApiCurrentToken())
            {
                Navigation.InsertPageBefore(new LoginPage(), this);
                Navigation.PopAsync();
            }
            
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iStudentClient = new StudentClient(apiClient);
        }

        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            _iTokenContainer.ClearApiCurrentToken();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }

        async void OnApiStudentListButtonClicked(object sender, EventArgs e)
        {
            var appMessage = new AppMessage();
            try
            {
                //var studentResponse = await _iStudentClient.GetStudent(1);
                var studentResponse = await _iStudentClient.GetStudentList();

                if (studentResponse.StatusIsSuccessful) {
                    //await DisplayAlert("Student", ("Name: " + studentResponse.Data.StudentName), "Ok");
                    await DisplayAlert("Student", ("Name: " + studentResponse.DataList.FirstOrDefault().StudentName), "Ok");
                }
                else {
                    await DisplayAlert("Error!", studentResponse.ErrorState.Message, "Ok");
                }
            }
            catch (Exception ex)
            {
                appMessage = appMessage.SetError();
                messageLabel.Text = appMessage.Message;
            }
            await Navigation.PopAsync();
        }

        async void OnPushNotificationButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new PushNotificationPage(), this);
            await Navigation.PopAsync();
        }
    }
}
