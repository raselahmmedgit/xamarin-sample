﻿// =========================================================================
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
using Covi.Features.Menu;
using Covi.Features.Menu.Components;
using Covi.Features.PrivacyPolicy.MenuComponents;
using Covi.Features.PrivacyPolicy.Routes;
using Covi.Features.PrivacyPolicy.Services;
using Covi.Features.Regions;
using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Features.PrivacyPolicy
{
    public class PrivacyPolicyModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterFunctionalityToTheRegion<PrivacyPolicyComponentService>(
                MenuViewModel.MenuRegionIdentifier,
                DataTemplateProviderFactory.CreatePlainFor<MenuItem>());

            containerRegistry.RegisterSingleton<IPrivacyPolicyService, PrivacyPolicyService>();
            containerRegistry.RegisterForNavigation<PrivacyPolicyPage, PrivacyPolicyViewModel>();
            containerRegistry.Register<IPrivacyPolicyRoute, PrivacyPolicyRoute>();
        }
    }
}
