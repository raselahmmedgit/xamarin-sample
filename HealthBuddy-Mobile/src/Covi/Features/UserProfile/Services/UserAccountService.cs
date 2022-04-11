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
using System.Threading;
using System.Threading.Tasks;
using Covi.Client.Services;
using Covi.Client.Services.Platform;
using Covi.Features.UserData.Services;
using Covi.Logs;
using Covi.Services.Http.Connectivity;
using Covi.Services.Http.ExceptionsHandling;
using Covi.Services.Localization;

using Microsoft.Extensions.Logging;

namespace Covi.Features.UserProfile.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IPlatformClient _platformClient;
        private readonly IErrorResponseHandler _serviceErrorHandler;
        private readonly IConnectivityService _connectivityService;
        private readonly IUserDataService _userDataService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        public UserAccountService(
            IPlatformClient platformClient,
            ILoggerFactory loggerFactory,
            IErrorResponseHandler serviceErrorHandler,
            IConnectivityService connectivityService,
            IUserDataService userDataService,
            ILocalizationService localizationService)
        {
            _platformClient = platformClient;
            _serviceErrorHandler = serviceErrorHandler;
            _connectivityService = connectivityService;
            _userDataService = userDataService;
            _localizationService = localizationService;
            _logger = loggerFactory.CreateLogger<UserAccountService>();
        }

        public async Task<Client.Services.Platform.Models.UserAccount> GetUserAccountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await GetUserAccountInternalAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to load user information.");
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw;
            }
        }

        private async Task<Client.Services.Platform.Models.UserAccount> GetUserAccountInternalAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var languageCode = _localizationService.CurrentCulture.TwoLetterISOLanguageName;
            _connectivityService.CheckConnection();
            var profileResponse = await _platformClient.Endpoints.GetUserProfileAsync(languageCode, cancellationToken).ConfigureAwait(false);
            var profile = profileResponse.UserAccount;
            await _userDataService.SetUserDataAsync(UserDataInfo.CreateFromResponse(profileResponse));

            return profile;
        }
    }
}
