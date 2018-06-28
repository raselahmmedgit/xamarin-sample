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
                //ontrackHealthWebView.Source = AppConstant.WebViewLoadUrl;

                //WebView webView = new WebView();
                //webView.Source = AppConstant.WebViewLoadUrl;
                //webView.VerticalOptions = LayoutOptions.FillAndExpand;
                //webView.HorizontalOptions = LayoutOptions.FillAndExpand;

                //Content = webView;

            }
            catch (Exception ex)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        //void webOnNavigating(object sender, WebNavigatingEventArgs e)
        //{
        //    try
        //    {
        //       var ontrackHealthWebView = this;
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }
        //}

        //void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
        //{
        //    try
        //    {
        //        var ontrackHealthWebView = this;
        //        var result = e.Result;
        //        var source = e.Source;
        //        var bindingContext = source.BindingContext;
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }
        //}

    }
}
