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
using Covi.Features.Menu;
using Covi.Features.Rumours.Actions;
using Covi.Utils.ReactiveCommandHelpers;
using MediatR;

namespace Covi.Features.Rumours.MenuComponents
{
    public class RumoursMenuItemViewModel : MenuItemViewModel
    {
        private const string RumoursIconSource = "resource://Covi.Features.Rumours.Resources.Images.rumours.svg";

        private readonly IMediator _mediator;

        public RumoursMenuItemViewModel(IMediator mediator)
        {
            _mediator = mediator;
            Title = Rumours.Resources.Localization.Menu_Rumours_ItemText;
            IconSource = RumoursIconSource;
            NavigateCommand = ReactiveCommandFactory.CreateLockedCommand(GoToRumoursAsync, nameof(MenuItemViewModel));
        }

        protected async Task GoToRumoursAsync()
        {
            await _mediator.Send(new NavigateToRumoursAction());
        }
    }
}
