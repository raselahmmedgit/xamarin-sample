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

using Prism.Modularity;
using Prism.Ioc;

namespace Covi.Features.Registry
{
    public static class AppFeatureRegistryExtensions
    {
        private static IModuleCatalog _moduleCatalog;

        public static App RegisterFeature<TFeature>(this App app, Action<IContainerProvider, TFeature> featureSetup = null)
            where TFeature : IFeature, new()
        {
            var moduleCatalog = GetModuleCatalog(app);
            var container = app.Container;

            var feature = new TFeature();
            feature.Register(container, moduleCatalog);
            if (featureSetup != null)
            {
                featureSetup.Invoke(container, feature);
            }
            else
            {
                feature.ConfigureDefault(container);
            }

            return app;
        }

        private static IModuleCatalog GetModuleCatalog(App app)
        {
            if (_moduleCatalog == null)
            {
                _moduleCatalog = app.Container.Resolve<IModuleCatalog>();
            }

            return _moduleCatalog;
        }
    }
}
