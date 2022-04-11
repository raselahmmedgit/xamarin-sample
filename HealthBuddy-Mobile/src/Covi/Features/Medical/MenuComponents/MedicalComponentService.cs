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
using System.Threading.Tasks;
using System.Collections.Generic;
using Covi.Features.Account.Services;
using Covi.Features.ComponentsManagement;
using Covi.Features.UserProfile.Services;
using Covi.Services.Feature;
using MediatR;
using Covi.Configuration;

namespace Covi.Features.Medical.MenuComponents
{
    public class MedicalComponentService : StatefulComponentServiceBase<MedicalState>
    {
        private const string HealthProfIconName = "show_password.svg";
        public override string ComponentKey => nameof(MedicalComponentService);

        private readonly IUserAccountContainer _userAccountContainer;
        private readonly IFeatureStateService _featureStateService;
        private readonly IEnvironmentConfiguration _environmentConfiguration;
        private readonly IMediator _mediator;

        public MedicalComponentService(
            IMediator mediator,
            IFeatureStateService featureStateService,
            IUserAccountContainer userAccountContainer,
            IEnvironmentConfiguration environmentConfiguration)
        {
            _mediator = mediator;
            _featureStateService = featureStateService;
            _userAccountContainer = userAccountContainer;
            _environmentConfiguration = environmentConfiguration;
            _featureStateService.FeatureConfigurationChanged.Subscribe(FeatureConfigurationChanged);
        }

        protected override IList<IComponent> UpdateState(MedicalState state)
        {
            if (state == null)
            {
                return new List<IComponent>();
            }

            var result = new List<IComponent>();

            switch (state.State)
            {
                case MedicalContentState.Enabled:
                    result.AddRange(GetMenuItems(state));
                    break;
                case MedicalContentState.Disabled:
                    break;
            }

            return result;
        }

        private async void FeatureConfigurationChanged(System.Reactive.Unit unit)
        {
            await UpdateCurrentStateAsync();
        }

        private async Task UpdateCurrentStateAsync()
        {
            MedicalState state = MedicalState.CreateDefaultState();
            if (_featureStateService.IsMedicalFeatureEnabled(_environmentConfiguration))
            {
                var profile = await _userAccountContainer.GetAsync().ConfigureAwait(false);
                var roles = profile?.UserAccount?.Roles;

                if (roles != null && roles.Contains(Roles.Medical))
                {
                    var menuItemsList = GetMedicalItemsList();

                    state = MedicalState.CreateMenuState(menuItemsList);
                }
            }

            SetState(state);
        }

        private IList<IComponent> GetMenuItems(MedicalState state)
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

        private IList<MedicalMenuItemViewModel> GetMedicalItemsList()
        {
            return new List<MedicalMenuItemViewModel>
            {
                new MedicalMenuItemViewModel(_mediator, Resources.Localization.Menu_HealthProf_ItemText, HealthProfIconName)
            };
        }
    }
}
