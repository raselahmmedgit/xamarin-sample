using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;

namespace lab.AppWebViewSample.Droid
{
	[Activity (Label = "lab.AppWebViewSample.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            //load url by WebView tools
            WebView webView = FindViewById<WebView>(Resource.Id.webViewOnTrackHealth);
            webView.SetWebViewClient(new WebViewClient());
            webView.LoadUrl(AppConstants.WebViewLoadUrl);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.SetSupportZoom(true);
            webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
            webView.ScrollbarFadingEnabled = true;

            ////open default browser
            //WebBrowser.OpenPage(this, AppConstants.WebViewLoadUrl);
        }
    }
}


