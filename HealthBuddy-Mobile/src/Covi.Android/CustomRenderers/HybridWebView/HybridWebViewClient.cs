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

using Android.Graphics;
using Android.Net.Http;
using Android.Webkit;

using Covi.Logs;

using Microsoft.Extensions.Logging;

using Xamarin.Forms.Platform.Android;

namespace Covi.Droid.CustomRenderers.HybridWebView
{
    public class HybridWebViewClient : FormsWebViewClient
    {
        private readonly string _javascript;
        private readonly HybridWebViewRenderer _renderer;
        private ILogger _logger;

        public HybridWebViewClient(HybridWebViewRenderer renderer, string javascript)
            : base(renderer)
        {
            _renderer = renderer;
            _javascript = javascript;
            _logger = Logger.Factory.CreateLogger("Android Web View");
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
            view.EvaluateJavascript(_javascript, null);
        }

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            _logger.LogError($"Chatbot loading error: {request.Url}, Reason: {error.Description}");
            base.OnReceivedError(view, request, error);
        }

        public override void OnReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
        {
            _logger.LogError($"Chatbot certificate error: {error.Url}, Reason: {error.PrimaryError.ToString()}.");
            base.OnReceivedSslError(view, handler, error);
        }
    }
}
