using App1720.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1720
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            try
            {
                WebView webView = new WebView();
                webView.Source = AppConstant.WebViewLoadUrl;
                webView.VerticalOptions = LayoutOptions.FillAndExpand;
                webView.HorizontalOptions = LayoutOptions.FillAndExpand;

                Content = webView;
            }
            catch (Exception ex)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

    }
}
