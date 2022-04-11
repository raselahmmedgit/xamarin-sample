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
using System.Linq;
using System.Net.Http;

using Covi.Client.Services;
using Covi.Features.Registry;
using Covi.Services.Http.AuthorizationHandling;
using Covi.Services.Http.LocalizationHandling;
using Covi.Services.Http.PlatformAuthenticationHandling;
using Covi.Services.Http.RequestIdHandling;
using Covi.Services.Http.SessionContainer;
using Covi.Services.Localization;
using Covi.Services.Platform;
using Covi.Services.Security;
using DryIoc;
using FFImageLoading;
using Prism.DryIoc.Extensions;
using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Services.Feature
{
    public class BaseServicesFeature : FeatureBase, IFeature
    {
        public override void Register(IContainerProvider registry, IModuleCatalog moduleCatalog)
        {
            // Application base services registration
            moduleCatalog.AddModule<Services.Serialization.SerializationModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<SecurityModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Services.Dispatcher.DispatcherModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Services.Storage.StorageModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Services.Notification.NotificationModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Services.ApplicationMetadata.MetadataModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Services.Platform.PlatformModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Services.ErrorHandlers.ErrorHandlerModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Features.AppSettings.AppSettingsModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Covi.Services.Dialogs.DialogModule>(InitializationMode.WhenAvailable);

            var container = registry.GetContainer();
            container.Register<IFeatureStateService, FeatureStateService>(Reuse.Singleton);
            container.Register<ILocalizationService, LocalizationService>(Reuse.Singleton);
        }
    }
}
