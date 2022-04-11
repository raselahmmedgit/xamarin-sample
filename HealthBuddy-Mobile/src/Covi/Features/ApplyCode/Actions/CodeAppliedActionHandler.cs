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

using Covi.Features.Main.Routes;
using Covi.Services.Navigation;
using MediatR;

namespace Covi.Features.ApplyCode
{
    public class CodeAppliedActionHandler : AsyncRequestHandler<CodeAppliedAction>, IRequestHandler<CodeAppliedAction, Unit>
    {
        private readonly IMainRoute _mainRoute;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        public CodeAppliedActionHandler(IMainRoute mainRoute, INavigationServiceDelegate navigationServiceDelegate)
        {
            _mainRoute = mainRoute;
            _navigationServiceDelegate = navigationServiceDelegate;
        }

        protected override async Task Handle(CodeAppliedAction request, CancellationToken cancellationToken)
        {
            await _mainRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
        }
    }
}
