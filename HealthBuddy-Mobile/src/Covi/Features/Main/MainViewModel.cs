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
using Covi.Features.Account.Services.Authentication;
using Covi.Features.PushNotifications.Services;
using Microsoft.Extensions.Logging;
using Prism.Navigation;

namespace Covi.Features.Main
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IPushNotificationsService _pushNotificationsService;
        private readonly IAuthenticationInfoService _authenticationInfoService;
        private readonly ILogger _logger;

        public MainViewModel(
            IPushNotificationsService pushNotificationsService,
            IAuthenticationInfoService authenticationInfoService,
            ILoggerFactory loggerFactory)
        {
            _pushNotificationsService = pushNotificationsService;
            _authenticationInfoService = authenticationInfoService;
            _logger = loggerFactory.CreateLogger<MainViewModel>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (_authenticationInfoService.IsAuthenticated()
                && !_authenticationInfoService.IsAnonymous())
            {
                _pushNotificationsService.UpdateTokenAsync().FireAndForget();
            }
        }
    }
}
