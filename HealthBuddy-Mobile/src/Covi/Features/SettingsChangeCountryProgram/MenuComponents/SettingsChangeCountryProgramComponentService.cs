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
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Covi.Features.ComponentsManagement;
using Covi.Features.UserProfile.Services;
using Prism.Ioc;

namespace Covi.Features.SettingsChangeCountryProgram.MenuComponents
{
    public class SettingsChangeCountryProgramComponentService : StatelessComponentServiceBase
    {
        private readonly IUserAccountContainer _userAccountContainer;
        private TaskCompletionSource<bool> _initializedSource = new TaskCompletionSource<bool>();

        public SettingsChangeCountryProgramComponentService(
            IContainerProvider containerProvider,
            IUserAccountContainer userAccountContainer)
            : base(containerProvider)
        {
            _userAccountContainer = userAccountContainer;

            // TaskCompletionSource is required to prevent any further execution until constructor is completed.
            _initializedSource.SetResult(true);
        }

        public override string ComponentKey { get => nameof(SettingsChangeCountryProgramComponentService); }

        protected override IObservable<IList<Type>> GetComponentTypes()
        {
            return Observable.FromAsync(async () => await GetComponentTypesByProfileAsync());
        }

        private async Task<IList<Type>> GetComponentTypesByProfileAsync()
        {
            await _initializedSource.Task;
            var profile = await _userAccountContainer.GetAsync().ConfigureAwait(false);
            var roles = profile?.UserAccount?.Roles;

            //if (roles != null && roles.Count > 0)
            //{
            //    return new List<Type>() { typeof(SettingsChangeCountryProgramMenuItemViewModel) };
            //}
            //else
            //{
            //    return new List<Type>();
            //}

            return new List<Type>() { typeof(SettingsChangeCountryProgramMenuItemViewModel) };
        }
    }
}
