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
	public partial class SignOutPage : ContentPage
	{
		public SignOutPage ()
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
	}
}