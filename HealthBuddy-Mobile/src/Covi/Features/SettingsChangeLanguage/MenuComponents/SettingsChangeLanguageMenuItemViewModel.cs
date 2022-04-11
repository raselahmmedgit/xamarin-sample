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
using Covi.Features.SettingsChangeLanguage.Actions;
using Covi.Utils.ReactiveCommandHelpers;

using MediatR;

namespace Covi.Features.SettingsChangeLanguage.MenuComponents
{
    public class SettingsChangeLanguageMenuItemViewModel : MenuItemViewModel
    {
        private const string SettingsChangeLanguageIconSource = "resource://Covi.Features.ChangeLanguage.Resources.Images.globe.svg";
        private readonly IMediator _mediator;

        public SettingsChangeLanguageMenuItemViewModel(IMediator mediator)
        {
            _mediator = mediator;
            Title = Covi.Features.ChangeLanguage.Resources.Localization.Menu_ChangeLanguage_ItemText;
            IconSource = SettingsChangeLanguageIconSource;
            NavigateCommand = ReactiveCommandFactory.CreateLockedCommand(GoToChangeLanguageAsync, nameof(MenuItemViewModel));
        }

        protected async Task GoToChangeLanguageAsync()
        {
            await _mediator.Send(new NavigateToSettingsChangeLanguageAction());
        }
    }
}
