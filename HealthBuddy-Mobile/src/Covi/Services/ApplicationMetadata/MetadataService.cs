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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covi.Client.Services;
using Covi.Client.Services.Platform;
using Covi.Client.Services.Platform.Models;
using Covi.Features.UserProfile.Services;
using Covi.Logs;
using Covi.Services.Http.Connectivity;
using Covi.Services.Http.ExceptionsHandling;
using Covi.Services.Localization;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

namespace Covi.Services.ApplicationMetadata
{
    public class MetadataService : IMetadataService
    {
        private readonly TimeSpan _metadataExpirationTime = Configuration.Constants.CacheExpirationConstants.MetadataExpirationTime;

        private readonly IMetadataContainer _metadataContainer;
        private readonly IPlatformClient _platformClient;
        private readonly IErrorResponseHandler _serviceErrorHandler;
        private readonly IConnectivityService _connectivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        private readonly SemaphoreSlim _metadataUpdateSemaphore = new SemaphoreSlim(1);

        public MetadataService(
            IMetadataContainer metadataContainer,
            IPlatformClient platformClient,
            IErrorResponseHandler serviceErrorHandler,
            IConnectivityService connectivityService,
            ILocalizationService localizationService,
            ILoggerFactory loggerFactory)
        {
            _metadataContainer = metadataContainer;
            _platformClient = platformClient;
            _serviceErrorHandler = serviceErrorHandler;
            _connectivityService = connectivityService;
            _localizationService = localizationService;
            _logger = loggerFactory.CreateLogger<UserAccountService>();
        }

        private DateTime LastUpdateDate { get; set; }

        public async Task<Metadata> FetchMetadataIfNeededAsync(bool forceUpdate = false)
        {
            try
            {
                using (await _metadataUpdateSemaphore.LockAsync())
                {
                    if (forceUpdate || IsExpired())
                    {
                        var result = await LoadDataInternalAsync().ConfigureAwait(false);

                        return result;
                    }
                    else
                    {
                        var result = await GetCachedMetadataAsync();
                        if (result == null)
                        {
                            result = await LoadDataInternalAsync().ConfigureAwait(false);
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to load metadata information.");
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw;
            }
        }

        public async Task<Metadata> TryFetchMetadataIfNeededAsync(bool forceUpdate = false)
        {
            try
            {
                return await FetchMetadataIfNeededAsync(forceUpdate);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Metadata> GetCachedMetadataAsync()
        {
            var result = await _metadataContainer.GetAsync().ConfigureAwait(false);
            if (result != null
                && string.Equals(_localizationService.CurrentCulture.TwoLetterISOLanguageName, result.LanguageCode))
            {
                return result.Metadata;
            }

            return null;
        }

        private bool IsExpired()
        {
            return _metadataExpirationTime < DateTime.Now - LastUpdateDate;
        }

        public async Task SetMetadataAsync(Metadata metadata)
        {
            var languageCode = _localizationService.CurrentCulture.TwoLetterISOLanguageName;
            var metadataModel = new MetadataModel()
            {
                Metadata = metadata,
                LanguageCode = languageCode
            };
            await SetMetadataModelAsync(metadataModel).ConfigureAwait(false);
        }

        private async Task SetMetadataModelAsync(MetadataModel metadata)
        {
            await _metadataContainer.SetAsync(metadata).ConfigureAwait(false);
            LastUpdateDate = DateTime.Now;
        }

        private async Task<Metadata> LoadDataInternalAsync(CancellationToken cancellationToken = default)
        {
            _connectivityService.CheckConnection();
            var languageCode = _localizationService.CurrentCulture.TwoLetterISOLanguageName;
            var metadataResponse = await _platformClient.Endpoints.GetApplicationMetadataAsync(languageCode, cancellationToken).ConfigureAwait(false);
            var metadataModel = new MetadataModel()
            {
                Metadata = metadataResponse.Metadata,
                LanguageCode = languageCode
            };
            await SetMetadataModelAsync(metadataModel).ConfigureAwait(false);

            return metadataResponse.Metadata;
        }
    }
}
