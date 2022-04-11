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

using System;
using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;
using Covi.Features.UserData.Services;
using Covi.Features.UserProfile.Services;
using Covi.Features.UserState.Services;
using Covi.Logs;
using Covi.Services.Http.SessionContainer;
using Covi.Services.Security.SecretsProvider;
using Microsoft.Extensions.Logging;

namespace Covi.Features.Account.Services.Authentication
{
    public class AuthenticationInfoService : IAuthenticationInfoService
    {
        private readonly IUserAccountContainer _userAccountContainer;
        private readonly ISessionContainer _sessionContainer;
        private readonly IUserDataService _userDataService;
        private readonly IUserStatusContainer _userStatusContainer;
        private readonly IAccountInformationContainer _accountInformationContainer;
        private readonly ILogger _logger;

        public AuthenticationInfoService(
            IUserAccountContainer userAccountContainer,
            ISessionContainer sessionContainer,
            IUserDataService userDataService,
            IUserStatusContainer userStatusContainer,
            IAccountInformationContainer accountInformationContainer,
            ILoggerFactory loggerFactory)
        {
            _userAccountContainer = userAccountContainer;
            _sessionContainer = sessionContainer;
            _userDataService = userDataService;
            _userStatusContainer = userStatusContainer;
            _accountInformationContainer = accountInformationContainer;
            _logger = loggerFactory.CreateLogger<AuthenticationInfoService>();
        }

        public async Task InitUserInfoAsync(Metadata metadata, UserAccountInfo userAccount, UserStatus userStatus, Token token)
        {
            var info = new UserDataInfo()
            {
                Metadata = metadata,
                UserAccountInfo = userAccount,
                UserStatus = userStatus
            };
            await _userDataService.SetUserDataAsync(info);
            await _sessionContainer.SetAsync(SessionInfo.CreateFromToken(token)).ConfigureAwait(false);
        }

        public bool IsAuthenticated()
        {
            var result = AsyncHelpers.RunSync(IsAuthenticatedAsync);
            return result;
        }

        public bool IsAnonymous()
        {
            var result = AsyncHelpers.RunSync(IsAnonymousAsync);
            return result;
        }

        private async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var isInitialized = SecretsProvider.Instance.GetIsInitialized();
                if (!isInitialized)
                {
                    return false;
                }

                var result = await _userAccountContainer.GetAsync().ConfigureAwait(false);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to check user authenticated status.");
                return false;
            }
        }

        private async Task<bool> IsAnonymousAsync()
        {
            try
            {
                var isInitialized = SecretsProvider.Instance.GetIsInitialized();
                if (!isInitialized)
                {
                    return false;
                }

                var result = await _userAccountContainer.GetAsync().ConfigureAwait(false);
                return result?.IsAnonymous ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to check user authenticated status.");
                return false;
            }
        }

        public async Task CleanUpUserInfoAsync()
        {
            // Metadata should be retained, so UserDataService.SetUserDataAsync() cannot be used.
            await _userAccountContainer.SetAsync(null).ConfigureAwait(false);
            await _userStatusContainer.SetAsync(null).ConfigureAwait(false);
            await _accountInformationContainer.SetAsync(null).ConfigureAwait(false);

            await _sessionContainer.SetAsync(null).ConfigureAwait(false);
        }
    }
}
