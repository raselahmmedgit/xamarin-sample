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
using Covi.Features.Account.Services.Authentication;
using Covi.Features.UserProfile.Services;
using Covi.Services.ApplicationMetadata;
using Covi.Services.Security.SecretsProvider;
using Microsoft.Extensions.Logging;

namespace Covi.Features.AnonymousProfile.Services
{
    public class AnonymousSignInService : IAnonymousSignInService
    {
        private readonly IMetadataService _metadataService;
        private readonly ISecretsProvider _secretsProvider;
        private readonly IAuthenticationInfoService _authenticationInfoService;
        private readonly IAnonymousSignInErrorResponseHandler _serviceErrorHandler;
        private readonly ILogger _logger;

        public AnonymousSignInService(
            IMetadataService metadataService,
            ISecretsProvider secretsProvider,
            IAuthenticationInfoService authenticationInfoService,
            IAnonymousSignInErrorResponseHandler anonymousSignInErrorResponseHandler,
            ILoggerFactory loggerFactory)
        {
            _metadataService = metadataService;
            _secretsProvider = secretsProvider;
            _authenticationInfoService = authenticationInfoService;
            _serviceErrorHandler = anonymousSignInErrorResponseHandler;
            _logger = loggerFactory.CreateLogger<AnonymousSignInService>();
        }

        public async Task AnonymousSignInAsync()
        {
            try
            {
                var generatedKey = Guid.NewGuid().ToString();
                await _secretsProvider.InitializeAsync(generatedKey).ConfigureAwait(false);

                var metadata = await _metadataService.FetchMetadataIfNeededAsync();

                UserAccountInfo userAccount = new UserAccountInfo();
                UserStatus userStatus = null;
                Token token = null;
                await _authenticationInfoService.InitUserInfoAsync(metadata, userAccount, userStatus, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var errorText = "Failed to sign in anonymously";
                _logger.LogError(ex, errorText);
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw new AnonymousSignInException(errorText, ex);
            }
        }
    }
}
