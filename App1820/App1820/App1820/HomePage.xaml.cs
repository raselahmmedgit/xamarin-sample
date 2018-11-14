using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1820
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
            #region iOS Apps Base Code
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    Title = "iOS Apps";
            //}
            //else if (Device.RuntimePlatform == Device.Android)
            //{
            //    Title = "Android Apps";
            //}
            //else if (Device.RuntimePlatform == Device.UWP)
            //{
            //    Title = "UWP Apps";
            //}
            #endregion

            Title = "Minnesota Urology";

            InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            #region iOS Apps Base Code
            if (Device.RuntimePlatform == Device.iOS)
            {
                iOSLabel.IsVisible = true;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                AndroidLabel.IsVisible = true;
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                UWPLabel.IsVisible = true;
            }
            #endregion
        }
    }
}