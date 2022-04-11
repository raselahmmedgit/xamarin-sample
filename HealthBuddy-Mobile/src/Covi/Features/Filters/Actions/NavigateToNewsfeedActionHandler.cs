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

using Covi.Features.Newsfeed.Routes;
using Covi.Services.Navigation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Covi.Features.Filters.Actions
{
    public class NavigateToNewsfeedActionHandler : AsyncRequestHandler<NavigateToNewsfeedAction>
    {
        private readonly INewsfeedRoute _newsfeedRoute;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        public NavigateToNewsfeedActionHandler(
            INewsfeedRoute newsfeedRoute,
            INavigationServiceDelegate navigationServiceDelegate
            )
        {
            _newsfeedRoute = newsfeedRoute;
            _navigationServiceDelegate = navigationServiceDelegate;
        }

        protected override async Task Handle(NavigateToNewsfeedAction request, CancellationToken cancellationToken)
        {
            await _newsfeedRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
        }
    }
}
