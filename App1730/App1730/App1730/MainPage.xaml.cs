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

        public MainPage()
		{
			InitializeComponent();

            var RuntimePlatform = Device.RuntimePlatform;
            var Idiom = Device.Idiom;
        }

        async void OnPushNotificationButtonClicked(object sender, EventArgs e)
        {
            //ILocalNotifications localNotifications = DependencyService.Get<ILocalNotifications>();
            //localNotifications.Show("Test", "Local notification alert", 1);

            CrossLocalNotifications.Current.Show("Test", "Local notification alert", 1, DateTime.Now.AddSeconds(0));
        }
    }
}
