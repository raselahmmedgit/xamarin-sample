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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Covi.Client.Services;
using Covi.Client.Services.Platform;
using Covi.Client.Services.Platform.Models;
using Covi.Configuration;
using Covi.Features.Filters;
using Covi.Features.Filters.Dialogs;
using Covi.Features.Filters.Services;
using Covi.Logs;
using Covi.Services.Http.Connectivity;
using Covi.Services.Http.ExceptionsHandling;
using Covi.Services.Localization;
using Covi.Services.Storage;
using Covi.Utils;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using Xamarin.Forms.Internals;

namespace Covi.Features.Newsfeed.Services
{
    public class NewsfeedService : INewsfeedService
    {
        private readonly IPlatformClient _platformClient;
        private readonly IErrorResponseHandler _serviceErrorHandler;
        private readonly IConnectivityService _connectivityService;
        private readonly IFiltersService _filtersService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<NewsfeedService> _logger;

        private const string FilterStorageKey = "FilterStorageKey";
        private readonly IStorageService _filtersStorageService;
        private IList<FilterDescription> _cachedFilters;
        private IList<FilterDescription> _filtersTemporary;

        private readonly TimeSpan NewsfeedExpirationTime;
        private string _cachedNewsFilterQuery;
        private NewsfeedArticles _cachedNews;
        private DateTime _lastNewsfeedUpdateDate;
        private bool _isFirstRun;
        private readonly SemaphoreSlim _newsfeedUpdateSemaphore = new SemaphoreSlim(1);

        public NewsfeedService(
            IPlatformClient platformClient,
            IErrorResponseHandler serviceErrorHandler,
            IConnectivityService connectivityService,
            IFiltersService filtersService,
            ILoggerFactory loggerFactory,
            IStorageService storageService,
            IEnvironmentConfiguration environmentConfiguration,
            ILocalizationService localizationService)
        {
            _platformClient = platformClient;
            _serviceErrorHandler = serviceErrorHandler;
            _connectivityService = connectivityService;
            _filtersService = filtersService;
            _filtersStorageService = storageService;
            _localizationService = localizationService;
            _logger = loggerFactory.CreateLogger<NewsfeedService>();
            NewsfeedExpirationTime = environmentConfiguration.GetNewsfeedCacheDuration();
        }

        public async Task<NewsfeedArticles> GetArticleListAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _connectivityService.CheckConnection();
                cancellationToken.ThrowIfCancellationRequested();

                var filtersQuery = await GetFiltersQueryAsync().ConfigureAwait(false);

                if (ShouldUpdateNewsfeed(filtersQuery))
                {
                    using (await _newsfeedUpdateSemaphore.LockAsync())
                    {
                        // Double lock to prevent mobile application from requesting the same data multiple times if method was called multiple times.
                        if (ShouldUpdateNewsfeed(filtersQuery))
                        {
                            // Request new articles and put it into memory cache
                            var articles = await _platformClient.Endpoints.GetArticlesAsync(filtersQuery, cancellationToken).ConfigureAwait(false);

                            _cachedNews = articles;
                            _cachedNewsFilterQuery = filtersQuery;
                            _lastNewsfeedUpdateDate = DateTime.Now;
                        }
                    }
                }

                // Newsfeed is valid, return from memory cache.
                return _cachedNews;
            }
            catch (Exception ex)
            {
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException, cancellationToken))
                {
                    if (!(ex is OperationCanceledException))
                    {
                        _logger.LogErrorExceptCancellation(ex, "Failed to load articles.");
                    }
                    generatedException.Rethrow();
                }

                throw;
            }
        }

        private async Task<string> GetFiltersQueryAsync()
        {
            var currentFilters = await GetFiltersAsync().ConfigureAwait(false);
            var filtersQuery = string.Join("&",
                currentFilters
                    .Where(f => f.Values?.Any() ?? false)
                    .Select(f =>
                        new
                        {
                            FilterName = f.ParameterName,
                            Values = f.Values
                                .Where(v => v.IsChecked)
                                .Select(v => v.Value)
                                .ToList()
                        })
                    .Select(f => $"{f.FilterName}={string.Join(",", f.Values)}")
                );

            // Replace query-meaningful symbols according to the contract with the backend.
            var encodedQuery = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(filtersQuery))
                .Replace("=", string.Empty)
                .Replace("/", "_")
                .Replace("+", "-");

            return encodedQuery;
        }

        private bool ShouldUpdateNewsfeed(string newFilterQuery)
        {
            var isSameQuery = string.Equals(newFilterQuery, _cachedNewsFilterQuery);
            var isExpired = NewsfeedExpirationTime < DateTime.Now - _lastNewsfeedUpdateDate;
            return !isSameQuery || isExpired;
        }

        public async Task<Article> GetArticleAsync(string articleId, string languageCode = null, CancellationToken cancellationToken = default)
        {
            try
            {
                languageCode ??= _localizationService.CurrentCulture.TwoLetterISOLanguageName;

                _connectivityService.CheckConnection();
                cancellationToken.ThrowIfCancellationRequested();

                var article = await _platformClient.Endpoints.GetArticleByIdAsync(articleId, languageCode, cancellationToken).ConfigureAwait(false);
                return article;
            }
            catch (Exception ex)
            {
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException, cancellationToken))
                {
                    if (!(ex is OperationCanceledException))
                    {
                        _logger.LogErrorExceptCancellation(ex, $"Failed to load the article with id={articleId}");
                    }
                    generatedException.Rethrow();
                }

                throw;
            }
        }

        public async Task<IList<FilterDescription>> GetFiltersAsync()
        {
            if (_cachedFilters == null)
            {
                var metadataFilters = await _filtersService.GetFiltersAsync().ConfigureAwait(false);
                var storedFilters = await _filtersStorageService.GetAsync<List<FilterDescription>>(FilterStorageKey).ConfigureAwait(false);
                if (storedFilters != null)
                {
                    // Migrate stored filter values to avoid the case when metadata filters has been changed due to locale change
                    ApplyFilters(storedFilters, metadataFilters);
                }

                await _filtersStorageService.AddAsync<List<FilterDescription>>(
                    FilterStorageKey,
                    metadataFilters.ToList());
                _cachedFilters = metadataFilters;
            }

            return _cachedFilters;
        }

        private void ApplyFilters(IList<FilterDescription> sourceFilters, IList<FilterDescription> targetFilters)
        {
            foreach (var sourceFilter in sourceFilters)
            {
                var targetFilter = targetFilters.FirstOrDefault(
                    x => x.ParameterName.Equals(sourceFilter.ParameterName, StringComparison.OrdinalIgnoreCase));
                if (targetFilter != null)
                {
                    foreach (var sourceValue in sourceFilter.Values)
                    {
                        var tagetValue = targetFilter.Values.FirstOrDefault(x => x.Value == sourceValue.Value);
                        if (tagetValue != null)
                        {
                            tagetValue.IsChecked = sourceValue.IsChecked;
                        }
                    }
                }
            }
        }

        public async Task<IList<FilterDescription>> ResetToLastSavedAsync()
        {
            _filtersTemporary = (await GetFiltersAsync().ConfigureAwait(false)).CloneValueList();
            return _filtersTemporary;
        }

        public async Task CommitFilterChangesAsync()
        {
            if (_filtersTemporary == null)
            {
                return;
            }

            _cachedFilters = _filtersTemporary.CloneValueList();
            await _filtersStorageService.AddAsync<List<FilterDescription>>(FilterStorageKey, _cachedFilters.ToList()).ConfigureAwait(false);
        }

        public IList<FilterDescription> SetFilterOption(FilterOptionItem filterOption)
        {
            _filtersTemporary.ForEach(f => f.Values
                .ForEach(i =>
                {
                    if (string.Equals(i.Value, filterOption.Value))
                    {
                        i.IsChecked = filterOption.IsChecked;
                    }
                }));

            return _filtersTemporary.ToList();
        }

        public async Task<IList<FilterDescription>> ResetToDefaultsAsync()
        {
            _filtersTemporary = await _filtersService.GetFiltersAsync().ConfigureAwait(false);
            return _filtersTemporary;
        }
    }
}
