﻿// =========================================================================
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
using System.Threading.Tasks;
using Covi.Client.Services;
using Covi.Client.Services.Platform;
using Covi.Client.Services.Platform.Models;
using Covi.Features.UserData.Services;
using Covi.Features.UserProfile.Services;
using Covi.Features.UserState.Services;
using Covi.Logs;
using Covi.Services.ApplicationMetadata;
using Covi.Services.Http.Connectivity;
using Microsoft.Extensions.Logging;

namespace Covi.Features.ApplyCode.Services
{
    public class UserStatusChangeService : IUserStatusChangeService
    {
        // Application makes a decision whether to upload device contacts or not based on this letter at the end of the status code change.
        private const string ShareCodeIndicator = "S";

        private readonly IPlatformClient _platformClient;
        private readonly IConnectivityService _connectivityService;
        private readonly IMeetingsService _meetingsService;
        private readonly ILogger _logger;
        private readonly IUserStatusChangeServiceErrorHandler _serviceErrorHandler;
        private readonly IUserDataService _userDataService;

        public UserStatusChangeService(
            IPlatformClient platformClient,
            ILoggerFactory loggerFactory,
            IConnectivityService connectivityService,
            IMeetingsService meetingsService,
            IUserStatusChangeServiceErrorHandler serviceErrorHandler,
            IUserDataService userDataService)
        {
            _platformClient = platformClient;
            _connectivityService = connectivityService;
            _meetingsService = meetingsService;
            _logger = loggerFactory.CreateLogger<UserStatusChangeService>();
            _serviceErrorHandler = serviceErrorHandler;
            _userDataService = userDataService;
        }

        public async Task<UserProfileResponse> ApplyStatusChangeCode(string code)
        {
            try
            {
                code = code.ToUpperInvariant();

                _connectivityService.CheckConnection();

                var meetings = ShouldShareDeviceContacts(code)
                    ? await _meetingsService.GetMeetingsAsync().ConfigureAwait(false)
                    : new List<Meeting>(0);

                await _userDataService.SetUserDataAsync(UserDataInfo.CreateFromResponse(new UserProfileResponse()));

                return new UserProfileResponse();
            }
            catch (Exception ex)
            {
                _logger.LogErrorExceptCancellation(ex, "Failed to apply the code.");
                if (_serviceErrorHandler.TryHandle(ex, out var generatedException))
                {
                    generatedException.Rethrow();
                }

                throw;
            }
        }

        private bool ShouldShareDeviceContacts(string code)
        {
            return code.EndsWith(ShareCodeIndicator, StringComparison.OrdinalIgnoreCase);
        }
    }
}
