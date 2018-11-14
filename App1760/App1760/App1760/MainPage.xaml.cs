using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Xamarin.Forms.Maps;

namespace App1760
{
	public partial class MainPage : ContentPage
	{
        //Geocoder geoCoder;
        private int CrossLocalNotificationId = 111;

        public MainPage()
		{
			InitializeComponent();
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

        async void OnMapsButtonClicked(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
