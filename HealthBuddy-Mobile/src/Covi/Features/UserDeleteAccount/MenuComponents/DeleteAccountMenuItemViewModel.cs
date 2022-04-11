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

using Covi.Features.Menu;
using Covi.Features.UserDeleteAccount.Actions;
using Covi.Utils.ReactiveCommandHelpers;
using MediatR;
using System.Threading.Tasks;

namespace Covi.Features.UserDeleteAccount.MenuComponents
{
    public class DeleteAccountMenuItemViewModel : MenuItemViewModel
    {
        private const string DeleteAccountIconSource = "resource://Covi.Features.UserDeleteAccount.Resources.Images.delete.svg";

        private readonly IMediator _mediator;

        public DeleteAccountMenuItemViewModel(IMediator mediator)
        {
            _mediator = mediator;

            Title = Resources.Localization.Menu_DeleteAccount_ItemText;
            IconSource = DeleteAccountIconSource;
            NavigateCommand = ReactiveCommandFactory.CreateLockedCommand(GoToDeleteAccountAsync, nameof(MenuItemViewModel));
        }

        protected async Task GoToDeleteAccountAsync()
        {
            await _mediator.Send(new NavigateToDeleteAccountAndDataAction());
        }
    }
}
