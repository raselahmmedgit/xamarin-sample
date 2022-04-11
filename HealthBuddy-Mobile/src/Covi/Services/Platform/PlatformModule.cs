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

using System.Linq;
using System.Net.Http;
using Covi.Client.Services;
using Covi.Services.Http.AuthorizationHandling;
using Covi.Services.Http.Connectivity;
using Covi.Services.Http.ExceptionsHandling;
using Covi.Services.Http.LocalizationHandling;
using Covi.Services.Http.PlatformAuthenticationHandling;
using Covi.Services.Http.RequestIdHandling;
using Covi.Services.Http.SessionContainer;
using DryIoc;
using FFImageLoading;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc.Extensions;
using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Services.Platform
{
    public class PlatformModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IConnectivityService, ConnectivityService>();
            containerRegistry.RegisterSingleton<IHttpExceptionContextRetriever, HttpExceptionContextRetriever>();
            containerRegistry.RegisterSingleton<IErrorResponseHandler, ErrorResponseHandler>();
            containerRegistry.RegisterSingleton<IRequestIdGenerator, RequestIdGenerator>();
            containerRegistry.RegisterSingleton<ISessionContainer, SessionContainer>();
            containerRegistry.RegisterSingleton<ITokenRefreshStrategy, TokenRefreshStrategy>();

            var container = containerRegistry.GetContainer();
            InitializeHttpHandlers(container);

            containerRegistry.RegisterSingletonFromDelegate<ITokenRefreshInvoker>(provider =>
            {
                var options = provider.GetRequiredService<Client.Services.PlatformClientOptions>();
                var handler = provider.GetRequiredService<Services.Http.IHttpClientHandlerProvider>()
                    .GetPlatformHandler();
                var handlers = HttpHandlersContainer.Instance.GetHandlers().ToArray();

                var client = Client.Services.ClientBuilder.Create(options, handler, handlers);
                return new TokenRefreshInvoker(client);
            });
            containerRegistry.RegisterSingletonFromDelegate<Covi.Client.Services.IPlatformClient>(
                (provider) =>
                {
                    var handler = provider.GetRequiredService<Services.Http.IHttpClientHandlerProvider>()
                        .GetPlatformHandler();
                    var options = provider.GetRequiredService<Client.Services.PlatformClientOptions>();

                    var handlers = HttpHandlersContainer.Instance.GetHandlers(nameof(Client.Services.IPlatformClient)).ToArray();

                    var client = Client.Services.ClientBuilder.Create(options, handler, handlers);
                    return client;
                });

            InitializeImageLoading(container);
        }

        private void InitializeHttpHandlers(IContainer container)
        {
            HttpHandlersContainer.Instance.AddHandler(() => new PlatformAuthenticationHandler());

            HttpHandlersContainer.Instance.AddHandler(() =>
            {
                var requestIdGenerator = container.Resolve<IRequestIdGenerator>();
                return new RequestIdDelegatingHandler(requestIdGenerator);
            });

            HttpHandlersContainer.Instance.AddScopedHandler(
                nameof(Client.Services.IPlatformClient),
                () =>
                {
                    var sessionContainer = container.Resolve<ISessionContainer>();
                    return new AuthDelegatingHandler(sessionContainer);
                });

            HttpHandlersContainer.Instance.AddScopedHandler(
                nameof(Client.Services.IPlatformClient),
                () =>
                {
                    var sessionContainer = container.Resolve<ISessionContainer>();
                    var torenRefreshStrategy = container.Resolve<ITokenRefreshStrategy>();
                    return new TokenRefreshDelegatingHandler(sessionContainer, torenRefreshStrategy);
                });

            HttpHandlersContainer.Instance.AddHandler(() => new AcceptLanguageDelegatingHandler());
        }

        private void InitializeImageLoading(IContainer container)
        {
            var imageService = ImageService.Instance;
            var handler = container.Resolve<Services.Http.IHttpClientHandlerProvider>()
                    .GetPlatformHandler();

            var delegatingHandlers = HttpHandlersContainer.Instance.GetHandlers().ToArray();

            handler = handler.AddHandlersChain(delegatingHandlers);

            imageService.Initialize(new FFImageLoading.Config.Configuration
            {
                HttpClient = new HttpClient(handler),
                FadeAnimationEnabled = true,
                FadeAnimationDuration = 150
            });
        }
    }
}
