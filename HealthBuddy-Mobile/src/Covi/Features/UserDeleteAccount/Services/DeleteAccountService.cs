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
using Covi.Features.Account.Services.Authentication;
using Covi.Features.Account.Services.SignOut;
using Covi.Features.Chat.Data;
using Covi.Logs;
using Covi.Services.Http.Connectivity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Covi.Features.UserDeleteAccount.Services
{
    public class DeleteAccountService : IDeleteAccountService
    {
        private readonly IPlatformClient _platformClient;
        private readonly IConnectivityService _connectivityService;
        private readonly IAuthenticationServiceErrorHandler _serviceErrorHandler;
        private readonly ISignOutService _signOutService;
        private readonly ChatDatabase _chatDatabase;

        private readonly ILogger _logger;

        public DeleteAccountService(
            IPlatformClient platformClient,
            IConnectivityService connectivityService,
            IAuthenticationServiceErrorHandler serviceErrorHandler,
            ISignOutService signOutService,
            ILoggerFactory loggerFactory)
        {
            _platformClient = platformClient;
            _connectivityService = connectivityService;
            _serviceErrorHandler = serviceErrorHandler;
            _signOutService = signOutService;
            _chatDatabase = new ChatDatabase();

            _logger = loggerFactory.CreateLogger<DeleteAccountService>();
        }

        public async Task DeleteUserAsync()
        {
            await DeleteUserDataAsync().ConfigureAwait(false);
            await _chatDatabase.DeleteAllRapidProMessageAsync();
            await _signOutService.SignOutAsync().ConfigureAwait(false);
        }

        private async Task DeleteUserDataAsync()
        {
            try
            {
                _connectivityService.CheckConnection();
                await _platformClient.Endpoints.DeleteUserWithHttpMessagesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to delete user account.");
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw;
            }
        }
    }
}
