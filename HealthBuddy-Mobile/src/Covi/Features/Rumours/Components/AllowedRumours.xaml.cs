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

using System.Threading.Tasks;
using Covi.Features.Controls.HybridWebView;
using Covi.Services.Http.UriHandlers;

using Xamarin.Forms;

namespace Covi.Features.Rumours.Components
{
    public partial class AllowedRumours : ContentView, IInteractionChannelHost
    {
        private readonly InteractionChannel _channel;

        public AllowedRumours()
        {
            InitializeComponent();
            _channel = new InteractionChannel(this);
            hybridWebView.React = s => _channel.HandleMessage(s);
            hybridWebView.Navigating += OnWebViewNavigating;
            hybridWebView.Navigated += OnWebViewNavigated;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _channel.SetHandler(BindingContext as IInteractionChannelHandler);
        }

        private void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            var ev = new UriOpeningConfig(e.Url);
            (BindingContext as RumoursViewModel)?.UriOpeningHandler.OnUriOpen(ev);
            e.Cancel = ev.Cancel;
        }

        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            switch (e.Result)
            {
                case WebNavigationResult.Success:
                    (BindingContext as IInteractionChannelHandler)?.ApiReadyMessage();
                    break;
                case WebNavigationResult.Failure:
                case WebNavigationResult.Timeout:
                    (BindingContext as RumoursViewModel)?.SetErrorState();
                    break;
            }
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            if (Xamarin.Essentials.MainThread.IsMainThread)
                return await hybridWebView.SendMessageAsync(message).ConfigureAwait(false);
            else
                return await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async () => await hybridWebView.SendMessageAsync(message)).ConfigureAwait(false);
        }
    }
}
