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

using Covi.Features.Components.BusyIndicator;
using Covi.Features.Regions;
using Covi.Utils;

using Xamarin.Forms;

namespace Covi.Features.ComponentsManagement
{
    public class ComponentDataTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _busyIndicatorDataTemplate;

        private DataTemplate BusyIndicatorDataTemplate
        {
            get => _busyIndicatorDataTemplate ??= new DataTemplate(() => DataTemplateCreationPreprocessor(new BusyIndicatorComponentView()));
        }

        protected override DataTemplate OnSelectTemplate(object item, Xamarin.Forms.BindableObject container)
        {
            if (item is IComponent component
                && container is ComponentsHost componentsHost
                && componentsHost.RegionManager != null)
            {
                if (TryGetRegisteredDataTemplate(component, componentsHost.RegionManager, out var dataTemplate))
                {
                    return dataTemplate;
                }

                // Global components handler.
                if (component is BusyIndicatorViewModel)
                {
                    return BusyIndicatorDataTemplate;
                }
            }

            throw new InvalidOperationException($"{nameof(ComponentDataTemplateSelector)} was unable to find DataTemplate for the item {item}.");
        }

        protected bool TryGetRegisteredDataTemplate(IComponent component, RegionManager regionManager, out DataTemplate dataTemplate)
        {
            dataTemplate = null;
            var result = RegionManager.TryGetDataTemplateProvider(regionManager.RegionIdentifier, component.ComponentGroupKey, out var dataTemplateProvider);
            if (result)
            {
                dataTemplate = dataTemplateProvider.GetDataTemplate(component, DataTemplateCreationPreprocessor);
                if (dataTemplate == null)
                {
                    result = false;
                }
            }

            return result;
        }

        protected virtual object DataTemplateCreationPreprocessor(View componentView)
        {
            return componentView.WrapWithViewCell();
        }
    }
}
