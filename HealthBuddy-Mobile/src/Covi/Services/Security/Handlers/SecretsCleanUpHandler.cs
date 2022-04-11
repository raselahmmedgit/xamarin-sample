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
using Covi.Services.Security.SecretsProvider;
using Microsoft.Extensions.Logging;

namespace Covi.Services.Security.Handlers
{
    public class SecretsCleanUpHandler : ICleanUpHandler
    {
        private readonly ISecretsProvider _secretsProvider;
        private readonly ILogger _logger;

        public SecretsCleanUpHandler(ISecretsProvider secretsProvider, ILoggerFactory loggerFactory)
        {
            _secretsProvider = secretsProvider;
            _logger = loggerFactory.CreateLogger<SecretsCleanUpHandler>();
        }

        public async Task InvokeAsync()
        {
            try
            {
                await _secretsProvider.CleanupAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Secrets clean up failed.");
            }
        }
    }
}
