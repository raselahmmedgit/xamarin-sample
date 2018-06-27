using App1720.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1720
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WebViewPage : ContentPage
	{
		public WebViewPage ()
		{
			InitializeComponent();

            try
            {
                //var htmlWebViewSource = new HtmlWebViewSource
                //{
                //    BaseUrl = AppConstant.WebViewLoadUrl
                //};
                ////ontrackHealthWebView.Source = htmlWebViewSource;
                //ontrackHealthWebView.Source = AppConstant.WebViewLoadUrl;

                //WebView webView = new WebView();
                //webView.Source = AppConstant.WebViewLoadUrl;
                //webView.VerticalOptions = LayoutOptions.FillAndExpand;
                //webView.HorizontalOptions = LayoutOptions.FillAndExpand;

                //Content = webView;
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }


        async void OnHomeButtonClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new MainPage());
        }
    }
}