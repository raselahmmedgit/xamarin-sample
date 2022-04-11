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
using Covi.Features.AboutUs.Actions;
using Covi.Features.Menu;
using Covi.Utils.ReactiveCommandHelpers;

using MediatR;

namespace Covi.Features.AboutUs.MenuComponents
{
    public class AboutUsMenuItemViewModel : MenuItemViewModel
    {
        private const string AboutUsIconSource = "resource://Covi.Features.AboutUs.Resources.Images.star.svg";

        private readonly IMediator _mediator;

        public AboutUsMenuItemViewModel(IMediator mediator)
        {
            _mediator = mediator;
            Title = AboutUs.Resources.Localization.Menu_AboutUs_ItemText;
            IconSource = AboutUsIconSource;
            NavigateCommand = ReactiveCommandFactory.CreateLockedCommand(GoToAboutUsAsync, nameof(MenuItemViewModel));
        }

        protected async Task GoToAboutUsAsync()
        {
            await _mediator.Send(new NavigateToAboutUsAction());
        }
    }
}
