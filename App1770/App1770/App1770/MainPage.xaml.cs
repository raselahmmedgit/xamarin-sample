using Acr.UserDialogs;
using App1770.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1770
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            Title = "Title";
		}

        private void btnToastButton_Clicked(object sender, EventArgs e)
        {
            //ToastHelper.ShowToastMessage("Hello World!...");

            ToastHelper.ShowPushNotification("Rasel", "Hello World!...", "1");
        }

        private void btnUserDialogsToastButton_Clicked(object sender, EventArgs e)
        {
            string str = string.Empty;

            UserDialogsToastHelper.ShowToastMessage("Hello World!...");
        }

        private void btnLoaderButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //LoaderHelper.ShowLoader(loaderStackLayout);

                string str = string.Empty;

                //LoaderHelper.HideLoader(loaderStackLayout);
            }
            
        }

        private void btnMenuButton_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                Navigation.PushAsync(new MenuPage());
            }

        }

    }
}
