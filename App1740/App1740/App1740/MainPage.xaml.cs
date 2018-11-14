using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1740
{
	public partial class MainPage : ContentPage
	{
        // Number of times the button is tapped (starts with first tap):
        private int CrossLocalNotificationId = 101;

        public MainPage()
		{
			InitializeComponent();

            DeviceRuntimePlatform();
        }

        async void OnLocalPushNotificationButtonClicked(object sender, EventArgs e)
        {
            try
            {
                //CrossLocalNotifications.Current.Show("Notify!", "Local notification alert...", CrossLocalNotificationId, DateTime.Now.AddSeconds(0));
                CrossLocalNotifications.Current.Show("Notify!", "Local notification alert...", CrossLocalNotificationId);
                //CrossLocalNotifications.Current.Cancel(CrossLocalNotificationId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async void DeviceRuntimePlatform()
        {
            try
            {
                string runtimePlatform = string.Empty;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        runtimePlatform = Device.RuntimePlatform;
                        //iOSLocalPushNotification();
                        break;
                    case Device.Android:
                        runtimePlatform = Device.RuntimePlatform;
                        //AndroidLocalPushNotification();
                        break;
                    case Device.UWP:
                        runtimePlatform = Device.RuntimePlatform;
                        //UWPLocalPushNotification();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        #region iOS Local Push Notification

        async void iOSLocalPushNotification()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Android Local Push Notification

        async void AndroidLocalPushNotification()
        {
            try
            {
                CrossLocalNotifications.Current.Show("Test", "Local notification alert", CrossLocalNotificationId, DateTime.Now.AddSeconds(0));
                CrossLocalNotifications.Current.Cancel(CrossLocalNotificationId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region UWP Local Push Notification

        async void UWPLocalPushNotification()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
