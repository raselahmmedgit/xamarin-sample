using Android.App;
using Android.Views;
using Android.OS;
using Android.Webkit;

namespace lab.AppNative.Droid
{
    [Activity (Label = "OnTrack Health", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Get our WebView from the layout resource,
            // and attach an event to it

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


