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
using Covi.Features.ChangeLanguage.Routes;
using Covi.Services.Navigation;
using MediatR;

namespace Covi.Features.ChangeLanguage.Actions
{
    class ChangeLanguageActionHandler : AsyncRequestHandler<NavigateToChangeLanguageAction>
    {
        private readonly IChangeLanguageRoute _changeLanguageRoute;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        public ChangeLanguageActionHandler(IChangeLanguageRoute changeLanguageRoute, INavigationServiceDelegate navigationServiceDelegate)
        {
            _changeLanguageRoute = changeLanguageRoute;
            _navigationServiceDelegate = navigationServiceDelegate;
        }

        protected override async Task Handle(NavigateToChangeLanguageAction request, CancellationToken cancellationToken)
        {
            await _changeLanguageRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
        }
    }
}
