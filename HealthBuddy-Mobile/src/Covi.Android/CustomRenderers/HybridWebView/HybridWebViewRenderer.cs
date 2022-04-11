// =========================================================================
// Copyright 2020 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================

using Android.Content;
using Android.Webkit;

using Covi.Droid.CustomRenderers.HybridWebView;
using Covi.Features.Controls.HybridWebView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace Covi.Droid.CustomRenderers.HybridWebView
{
    public class HybridWebViewRenderer : WebViewRenderer
    {
        private const string JavascriptFunction = @"window[""dispatchNative""] = function(data){jsBridge.invokeAction(data);}";

        public HybridWebViewRenderer(Context context)
            : base(context)
        {
        }

        protected override Android.Webkit.WebView CreateNativeControl()
        {
            var result = new Android.Webkit.WebView(GetFixedContext(Context));

            return result;
        }

        private Context GetFixedContext(Context context)
        {
            // This is a workaround for AppCompat issue with WebView initialization for Android 5.0 and 5.1
            // https://issuetracker.google.com/issues/141132133
            // https://stackoverflow.com/questions/41025200/android-view-inflateexception-error-inflating-class-android-webkit-webview
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop
                && Android.OS.Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.LollipopMr1)
            {
                return context.CreateConfigurationContext(new Android.Content.Res.Configuration());
            }

            return context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                CleanUp(e.OldElement);
            }
            if (e.NewElement != null)
            {
                SetUp();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                ((Covi.Features.Controls.HybridWebView.HybridWebView)Element).Cleanup();
            }
            base.Dispose(disposing);
        }

        private void CleanUp(Xamarin.Forms.WebView element)
        {
            Control.RemoveJavascriptInterface("jsBridge");
            if (element is Covi.Features.Controls.HybridWebView.HybridWebView hybridWebView)
            {
                hybridWebView.Cleanup();
            }
        }

        private void SetUp()
        {
            Control.SetWebViewClient(new HybridWebViewClient(this, $"javascript: {JavascriptFunction}"));
            Control.AddJavascriptInterface(new JsBridge(this), "jsBridge");
            Control.SetBackgroundColor(new Android.Graphics.Color(0x00000000));
            Control.Settings.CacheMode = CacheModes.NoCache;
        }
    }
}
