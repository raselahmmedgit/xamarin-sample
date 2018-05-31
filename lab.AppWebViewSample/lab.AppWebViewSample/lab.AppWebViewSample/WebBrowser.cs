using Android.App;
using Android.Content;
using System;

namespace lab.AppWebViewSample
{
    public class WebBrowser
    {
        public static void OpenPage(Activity context, String url)
        {
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            context.StartActivity(intent);
        }
    }
}

