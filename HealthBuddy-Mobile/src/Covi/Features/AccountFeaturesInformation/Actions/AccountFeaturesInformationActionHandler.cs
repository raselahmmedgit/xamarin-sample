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

using System.Threading;
using System.Threading.Tasks;
using Covi.Features.AccountFeaturesInformation.Routes;
using Covi.Services.Navigation;
using MediatR;

namespace Covi.Features.AccountFeaturesInformation
{
    public class AccountFeaturesInformationActionHandler : AsyncRequestHandler<AccountFeaturesInformationAction>, IRequestHandler<AccountFeaturesInformationAction, Unit>
    {
        private readonly IAccountInformationRoute _accountInformationRoute;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        public AccountFeaturesInformationActionHandler(IAccountInformationRoute accountInformationRoute, INavigationServiceDelegate navigationServiceDelegate)
        {
            _accountInformationRoute = accountInformationRoute;
            _navigationServiceDelegate = navigationServiceDelegate;
        }

        protected override async Task Handle(AccountFeaturesInformationAction request, CancellationToken cancellationToken)
        {
            await _accountInformationRoute.ExecuteAsync(_navigationServiceDelegate, request.AccountType).ConfigureAwait(false);
        }
    }
}
