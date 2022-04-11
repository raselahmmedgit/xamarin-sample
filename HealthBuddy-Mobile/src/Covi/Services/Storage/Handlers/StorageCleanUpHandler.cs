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
using Covi.Services.CleanUp.Handlers;
using Covi.Services.Storage.LiteDbStorage;
using Microsoft.Extensions.Logging;

namespace Covi.Services.Storage.Handlers
{
    public class StorageCleanUpHandler : ICleanUpHandler
    {
        private readonly IStorageService _storageService;
        private readonly IStorageOptionsProvider _storageOptionsProvider;
        private readonly ILogger _logger;

        public StorageCleanUpHandler(IStorageService storageService, IStorageOptionsProvider storageOptionsProvider, ILoggerFactory loggerFactory)
        {
            _storageService = storageService;
            _storageOptionsProvider = storageOptionsProvider;
            _logger = loggerFactory.CreateLogger<StorageCleanUpHandler>();
        }

        public async Task InvokeAsync()
        {
            try
            {
                await _storageService.DropStorageAsync().ConfigureAwait(false);
                _storageOptionsProvider.ResetCache();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Storage clean up failed.");
            }
        }
    }
}
