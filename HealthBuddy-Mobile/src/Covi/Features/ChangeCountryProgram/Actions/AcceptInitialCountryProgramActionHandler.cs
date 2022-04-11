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
using System.Threading;
using System.Threading.Tasks;
using Covi.Features.Account.Services.Authentication;
using Covi.Features.Main.Routes;
using Covi.Features.Welcome.Routes;
using Covi.Services.Navigation;

using MediatR;

namespace Covi.Features.ChangeCountryProgram.Actions
{
    public class AcceptInitialCountryProgramActionHandler : AsyncRequestHandler<AcceptInitialCountryProgramAction>
    {
        private readonly IWelcomeRoute _welcomeRoute;
        private readonly IMainRoute _mainRoute;
        private readonly IAuthenticationInfoService _authenticationInfoService;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        public AcceptInitialCountryProgramActionHandler(IWelcomeRoute welcomeRoute
            , IMainRoute mainRoute
            , IAuthenticationInfoService authenticationInfoService
            , INavigationServiceDelegate navigationServiceDelegate)
        {
            _welcomeRoute = welcomeRoute;
            _mainRoute = mainRoute;
            _authenticationInfoService = authenticationInfoService;
            _navigationServiceDelegate = navigationServiceDelegate;
        }

        protected override async Task Handle(AcceptInitialCountryProgramAction request, CancellationToken cancellationToken)
        {
            if (_authenticationInfoService.IsAuthenticated()
                && !_authenticationInfoService.IsAnonymous())
            {
                await _mainRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
            }
            else
            {
                await _welcomeRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
            }
        }
    }
}
