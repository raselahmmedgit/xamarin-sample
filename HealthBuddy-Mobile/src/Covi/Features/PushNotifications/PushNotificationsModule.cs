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

using Covi.Client.Services;
using Covi.Features.ComponentsManagement;
using Covi.Features.PushNotifications.Handlers;
using Covi.Features.PushNotifications.Services;
using Covi.Features.PushNotifications.SettingsComponents;
using Covi.Features.Regions;
using Covi.Features.Settings;
using Covi.Features.Settings.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Features.PushNotifications
{
    public class PushNotificationsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingletonFromDelegate<IPushNotificationsService>(provider =>
            {
                var service = new PushNotificationsService(
                    provider.GetRequiredService<IPlatformClient>(),
                    provider.GetRequiredService<ILoggerFactory>());
                service.AddInitializer(provider.GetService<IPushNotificationInitializer>());
                return service;
            });
        }
    }
}
