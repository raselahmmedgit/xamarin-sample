using Plugin.LocalNotifications;
using Plugin.LocalNotifications.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1730
{
	public partial class MainPage : ContentPage
	{
        public string Message
        {
            get
            {
                return textLabel.Text;
            }
            set
            {
                textLabel.Text = value;
            }
        }

        // Number of times the button is tapped (starts with first tap):
        private int count = 1;

        public MainPage()
		{
			InitializeComponent();

            DeviceRuntimePlatform();

        }

        async void DeviceRuntimePlatform()
        {
            string runtimePlatform = string.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    runtimePlatform = Device.RuntimePlatform;
                    break;
                case Device.Android:
                    runtimePlatform = Device.RuntimePlatform;
                    break;
                case Device.UWP:
                    runtimePlatform = Device.RuntimePlatform;
                    break;
                default:
                    break;
            }
        }

        #region iOS Local Push Notification

        async void iOSLocalPushNotification()
        {
            
        }

        #endregion

        #region Android Local Push Notification

        async void AndroidLocalPushNotification()
        {

        }

        #endregion

        #region UWP Local Push Notification

        async void UWPLocalPushNotification()
        {

        }

        #endregion

        async void OnPushNotificationButtonClicked(object sender, EventArgs e)
        {
            //ILocalNotifications localNotifications = DependencyService.Get<ILocalNotifications>();
            //localNotifications.Show("Test", "Local notification alert", 1);

            CrossLocalNotifications.Current.Show("Test", "Local notification alert", 1, DateTime.Now.AddSeconds(0));
        }
    }
}
