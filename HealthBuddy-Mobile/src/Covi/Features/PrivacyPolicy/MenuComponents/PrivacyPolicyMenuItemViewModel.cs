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
using Covi.Features.PrivacyPolicy.Actions;
using Covi.Utils.ReactiveCommandHelpers;
using MediatR;

namespace Covi.Features.PrivacyPolicy.MenuComponents
{
    public class PrivacyPolicyMenuItemViewModel : MenuItemViewModel
    {
        private const string PrivacyPolicyIconSource = "resource://Covi.Features.PrivacyPolicy.Resources.Images.key.svg";

        private readonly IMediator _mediator;

        public PrivacyPolicyMenuItemViewModel(IMediator mediator)
        {
            _mediator = mediator;
            Title = Resources.Localization.Menu_PrivacyPolicy_ItemText;
            IconSource = PrivacyPolicyIconSource;
            NavigateCommand = ReactiveCommandFactory.CreateLockedCommand(GoToPrivacyPolicyAsync, nameof(MenuItemViewModel));
        }

        protected async Task GoToPrivacyPolicyAsync()
        {
            await _mediator.Send(new NavigateToPrivacyPolicyAction());
        }
    }
}
