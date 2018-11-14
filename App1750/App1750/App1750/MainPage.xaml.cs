using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1750
{
	public partial class MainPage : ContentPage
	{
        private int CrossNotificationId = 111;

        public MainPage()
		{
			InitializeComponent();
		}

        async void OnLocalPushNotificationButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Plugin.Notifications.Notification notification = new Plugin.Notifications.Notification();
                notification.Id = CrossNotificationId;
                notification.Title = "Notify!";
                notification.Message = "Local notification alert...";
                //notification.When = DateTime.Now.TimeOfDay;

                await Plugin.Notifications.CrossNotifications.Current.Send(notification);
                await Plugin.Notifications.CrossNotifications.Current.Cancel(CrossNotificationId);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
