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
using Covi.Features.Filters.Dialogs;
using Covi.Logs;
using Covi.Services.ApplicationMetadata;
using Covi.Services.Http.Connectivity;
using Covi.Services.Http.ExceptionsHandling;
using Microsoft.Extensions.Logging;

namespace Covi.Features.Filters.Services
{
    public class FiltersService : IFiltersService
    {
        private readonly IPlatformClient _platformClient;
        private readonly IMetadataService _metadataService;
        private readonly IErrorResponseHandler _serviceErrorHandler;
        private readonly ILogger _logger;

        public FiltersService(
            IPlatformClient platformClient,
            IMetadataService metadataService,
            ILoggerFactory loggerFactory,
            IErrorResponseHandler serviceErrorHandler)
        {
            _platformClient = platformClient;
            _metadataService = metadataService;
            _serviceErrorHandler = serviceErrorHandler;
            _logger = loggerFactory.CreateLogger<FiltersService>();
        }

        public async Task<IList<FilterDescription>> GetFiltersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await LoadFiltersInternalAsync(cancellationToken).ConfigureAwait(false);

                return result
                    .Select(f => f.ToFilterDescription())
                    .Where(f => f != null)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to load filters.");
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw;
            }
        }

        private async Task<IList<Filter>> LoadFiltersInternalAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var metadata = await _metadataService.GetCachedMetadataAsync().ConfigureAwait(false);
            if (metadata == null)
            {
                metadata = await _metadataService.FetchMetadataIfNeededAsync().ConfigureAwait(false);
            }

            var filters = metadata.Filters;

            return filters;
        }
    }
}
