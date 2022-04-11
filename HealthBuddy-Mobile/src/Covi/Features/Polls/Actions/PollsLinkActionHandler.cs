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

using Covi.Features.Main;
using Covi.Services.Actions;
using Covi.Services.Notification;

using MediatR;

namespace Covi.Features.Polls
{
    public class PollsLinkActionHandler : AsyncRequestHandler<PollsAction>, IRequestHandler<PollsAction, Unit>
    {
        private IMediator _mediator;
        private PollsPageProviderService _pollsPageProviderService;

        public PollsLinkActionHandler(IMediator mediator, PollsPageProviderService pollsPageProviderService)
        {
            _mediator = mediator;
            _pollsPageProviderService = pollsPageProviderService;
        }

        protected override async Task Handle(PollsAction request, CancellationToken cancellationToken)
        {
            _pollsPageProviderService.SetDefault(true);
            if (App.Current.MainPage is Shell.ShellPage shell
                && shell.CurrentPage is MainPage mainPage)
            {
                mainPage.UpdateCurrentPage(_pollsPageProviderService.PageType);
            }
            else
            {
                await _mediator.Send(new AppInitializedAction());
            }
        }
    }
}
