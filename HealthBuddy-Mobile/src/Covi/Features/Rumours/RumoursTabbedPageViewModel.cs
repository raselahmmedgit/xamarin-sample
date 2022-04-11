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

using System.Threading.Tasks;
using Covi.Features.Rumours.Actions;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Covi.Features.Rumours
{
    public class RumoursTabbedPageViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;

        public RumoursTabbedPageViewModel(IMediator mediator)
        {
            _mediator = mediator;

            NavigateToRumours = ReactiveCommand.CreateFromTask(NavigateToRumoursAsync);
        }

        public ReactiveCommand<Unit, Unit> NavigateToRumours { get; }

        private async Task NavigateToRumoursAsync()
        {
            await _mediator.Send(new NavigateToRumoursAction());
        }
    }
}
