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

using Covi.Client.Services;
using Covi.Client.Services.Platform;
using Covi.Features.Account.Services.Authentication;
using Covi.Logs;
using Covi.Services.CleanUp.Handlers;
using Covi.Services.Http.Connectivity;
using Covi.Services.Http.SessionContainer;
using Covi.Services.Security.SecretsProvider;

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Covi.Features.Account.Services.SignOut
{
    public class SignOutService : ISignOutService
    {
        private const int SignOutMaxDelayMilliseconds = 1000;

        private readonly IPlatformClient _platformClient;
        private readonly IAuthenticationServiceErrorHandler _serviceErrorHandler;
        private readonly IConnectivityService _connectivityService;
        private readonly ISessionContainer _sessionInfoContainer;
        private readonly ICleanUpHandlersRegistry _cleanUpHandlersRegistry;
        private readonly ISecretsProvider _secretsProvider;
        private readonly ILogger _logger;

        public SignOutService(
            ICleanUpHandlersRegistry cleanUpHandlersRegistry,
            ILoggerFactory loggerFactory,
            IPlatformClient platformClient,
            IAuthenticationServiceErrorHandler serviceErrorHandler,
            IConnectivityService connectivityService,
            ISessionContainer sessionInfoContainer,
            ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider;
            _platformClient = platformClient;
            _serviceErrorHandler = serviceErrorHandler;
            _connectivityService = connectivityService;
            _sessionInfoContainer = sessionInfoContainer;
            _cleanUpHandlersRegistry = cleanUpHandlersRegistry;
            _logger = loggerFactory.CreateLogger<SignOutService>();
        }

        public async Task SignOutAsync()
        {
            var isInitialized = _secretsProvider.GetIsInitialized();
            if (!isInitialized)
            {
                // No need to logout before any information is stored.
                return;
            }

            var logoutTask = TrySignOutFromServerAsync();

            // Limit the logout backend call to the timeout.
            await logoutTask.ReturnInTimeoutAsync(true, SignOutMaxDelayMilliseconds);

            //put logout logic here. then handlers will do all the cleanup work
            foreach (var step in _cleanUpHandlersRegistry.GetHandlers())
            {
                try
                {
                    await step.InvokeAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Logout step {step.GetType().Name} failed.");
                }
            }
        }

        private async Task<bool> TrySignOutFromServerAsync()
        {
            try
            {
                await SignOutFromServerAsync().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task SignOutFromServerAsync()
        {
            try
            {
                _connectivityService.CheckConnection();
                var session = await _sessionInfoContainer.GetAsync().ConfigureAwait(false);
                var refreshToken = session?.RefreshToken;
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _platformClient.Endpoints.LogoutUserAsync(new Client.Services.Platform.Models.RefreshTokenRequest(refreshToken)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to sign out.");
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw;
            }
        }
    }
}
