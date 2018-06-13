using App1710.ApiHelper;
using App1710.ApiHelper.Client;
using App1710.ApiService.Client;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1710
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PushNotificationPage : ContentPage
	{
        const int _SAMPLE_ID = 1;
        int _secondsToDelivery;

        private readonly IStudentClient _iStudentClient;
        private readonly ITokenContainer _iTokenContainer;

        public PushNotificationPage ()
		{
			InitializeComponent ();
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

        void ScheduledSwitchToggled(object sender, ToggledEventArgs e)
        {
            ScheduleSecondsPicker.IsVisible = ScheduleSwitch.IsToggled;

            if (!ScheduleSwitch.IsToggled)
            {
                _secondsToDelivery = 0;
            }
        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _secondsToDelivery = int.Parse(ScheduleSecondsPicker.Items[ScheduleSecondsPicker.SelectedIndex]);
        }

        void SendButtonClicked(object sender, EventArgs e)
        {
            if (_secondsToDelivery > 0)
            {
                CrossLocalNotifications.Current.Show(TitleEntry.Text, BodyEntry.Text, _SAMPLE_ID, DateTime.Now.AddSeconds(_secondsToDelivery));
            }
            else
            {
                CrossLocalNotifications.Current.Show(TitleEntry.Text, BodyEntry.Text, _SAMPLE_ID);
            }
        }

        void CancelButtonClicked(object sender, EventArgs e)
        {
            CrossLocalNotifications.Current.Cancel(_SAMPLE_ID);
        }
    }
}