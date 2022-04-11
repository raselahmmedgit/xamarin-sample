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
using Covi.Features.Account.Services.Authentication;
using Covi.Services.CleanUp.Handlers;
using Microsoft.Extensions.Logging;

namespace Covi.Features.Account.Handlers
{
    public class AuthenticationInfoCleanUpHandler : ICleanUpHandler
    {
        private readonly IAuthenticationInfoService _authenticationInfoService;
        private readonly ILogger _logger;

        public AuthenticationInfoCleanUpHandler(IAuthenticationInfoService authenticationInfoService, ILoggerFactory loggerFactory)
        {
            _authenticationInfoService = authenticationInfoService;
            _logger = loggerFactory.CreateLogger<AuthenticationInfoCleanUpHandler>();
        }

        public async Task InvokeAsync()
        {
            try
            {
                await _authenticationInfoService.CleanUpUserInfoAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Authentication info cleanup failed.");
            }
        }
    }
}
