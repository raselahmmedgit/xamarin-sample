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

using Xamarin.Forms;

namespace Covi.Features.ComponentsManagement
{
    public abstract class CompositeDataTemplateProvider : DataTemplateProviderBase
    {
        private Dictionary<Func<View>, WeakReference<DataTemplate>> _dataTemplateCache = new Dictionary<Func<View>, WeakReference<DataTemplate>>();

        public override DataTemplate GetDataTemplate(IComponent component, Func<View, object> preprocessor = null)
        {
            var viewProvider = GetViewProvider(component);
            if (viewProvider == null)
            {
                return null;
            }

            if (_dataTemplateCache.TryGetValue(viewProvider, out var dataTemplateReference)
                && dataTemplateReference.TryGetTarget(out var dataTemplate))
            {
                return dataTemplate;
            }

            dataTemplate = CreateDataTemplate(viewProvider, preprocessor);
            _dataTemplateCache[viewProvider] = new WeakReference<DataTemplate>(dataTemplate);
            return dataTemplate;
        }

        protected abstract Func<View> GetViewProvider(IComponent component);
    }
}
