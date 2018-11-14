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
	public partial class PhysicianPage : ContentPage
	{
		public PhysicianPage ()
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

            try
            {
                //var htmlWebViewSource = new HtmlWebViewSource
                //{
                //    BaseUrl = AppConstant.WebViewLoadUrl
                //};
                //ontrackHealthWebView.Source = htmlWebViewSource;

                ontrackHealthWebView.Source = AppConstant.WebViewLoadUrl;

                //WebView webView = new WebView();
                //webView.Source = AppConstant.WebViewLoadUrl;
                //webView.VerticalOptions = LayoutOptions.FillAndExpand;
                //webView.HorizontalOptions = LayoutOptions.FillAndExpand;
                //webView.HeightRequest = 600;

                //ontrackHealthWebViewStackLayout.Children.Add(webView);

                //Content = webView;
            }
            catch (Exception ex)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {

        }
    }
}