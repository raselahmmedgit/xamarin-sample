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

using Covi.Features.ComponentsManagement;
using Covi.Features.UserProfile.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covi.Features.UserDeleteAccount.MenuComponents
{
    public class DeleteAccountComponentService : StatefulComponentServiceBase<DeleteAccountState>
    {
        private readonly IUserAccountContainer _userAccountContainer;
        private readonly IMediator _mediator;

        public override string ComponentKey => nameof(DeleteAccountComponentService);

        public DeleteAccountComponentService(
            IMediator mediator,
            IUserAccountContainer userAccountContainer)
        {
            _mediator = mediator;
            _userAccountContainer = userAccountContainer;
        }

        protected override IList<IComponent> UpdateState(DeleteAccountState state)
        {
            if (state == null)
            {
                return new List<IComponent>();
            }

            var result = new List<IComponent>();

            switch (state.State)
            {
                case DeleteAccountContentState.LoggedIn:
                    result.AddRange(GetMenuItems(state));
                    break;
                case DeleteAccountContentState.Default:
                    break;
            }

            return result;
        }

        private IList<IComponent> GetMenuItems(DeleteAccountState state)
        {
            var result = new List<IComponent>();
            foreach (var menuItem in state.MenuItemsList)
            {
                result.Add(menuItem);
            }

            return result;
        }

        protected override async void OnActivated()
        {
            base.OnActivated();
            await UpdateCurrentStateAsync();
        }

        private async Task UpdateCurrentStateAsync()
        { 
            var profile = await _userAccountContainer.GetAsync().ConfigureAwait(false);
            var roles = profile?.UserAccount?.Roles;

            if (roles != null && roles.Count > 0)
            {
                var menuItemsList = await GetDeleteAccountItemsListAsync();

                SetState(DeleteAccountState.CreateMenuState(menuItemsList));
            }
        }

        private Task<IList<DeleteAccountMenuItemViewModel>> GetDeleteAccountItemsListAsync()
        {
            IList<DeleteAccountMenuItemViewModel> items = new List<DeleteAccountMenuItemViewModel>
            {
                new DeleteAccountMenuItemViewModel(_mediator)
            };

            return Task.FromResult(items);
        }
    }
}
