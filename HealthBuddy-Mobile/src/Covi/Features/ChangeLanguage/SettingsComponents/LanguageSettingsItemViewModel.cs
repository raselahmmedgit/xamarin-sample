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
using Covi.Features.SettingsChangeLanguage.Actions;
using Covi.Utils.ReactiveCommandHelpers;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Covi.Features.ChangeLanguage.SettingsComponents
{
    public class LanguageSettingsItemViewModel : ComponentViewModelBase
    {
        private readonly IMediator _mediator;

        public ReactiveCommand<Unit, Unit> SettingsLanguageCommand { get; }

        public LanguageSettingsItemViewModel(IMediator mediator)
        {
            _mediator = mediator;
            SettingsLanguageCommand = ReactiveCommandFactory.CreateLockedCommand(GoToLanguageSettingsAsync);
        }

        private async Task GoToLanguageSettingsAsync()
        {
            await _mediator.Send(new NavigateToSettingsChangeLanguageAction());
        }

    }
}
