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
using Covi.Features.Regions;
using Covi.Features.UserDeleteAccount.MenuComponents;
using Covi.Features.UserDeleteAccount.Routes;
using Covi.Features.UserDeleteAccount.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Features.UserDeleteAccount
{
    public class DeleteAccountModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterFunctionalityToTheRegion<DeleteAccountComponentService>(
                MenuViewModel.MenuRegionIdentifier, new DeleteAccountDataTemplateProvider());

            containerRegistry.RegisterForNavigation<DeleteAccountPage, DeleteAccountViewModel>();
            containerRegistry.Register<IDeleteAccountRoute, DeleteAccountRoute>();

            containerRegistry.Register<IDeleteAccountService, DeleteAccountService>();
        }
    }
}
