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

using Covi.Utils;

using Xamarin.Forms;

namespace Covi.Features.ComponentsManagement
{
    public class PlainDataTemplateProvider<TView> : DataTemplateProviderBase
        where TView : View
    {
        // DataTemplate caching is essential for Android, application will crash otherwise.
        private WeakReference<DataTemplate> _dataTemplate;
        private Func<TView> _viewProvider;

        internal PlainDataTemplateProvider(Func<TView> viewProvider)
        {
            _viewProvider = viewProvider;
        }

        public override DataTemplate GetDataTemplate(IComponent component, Func<View, object> preprocessor = null)
        {
            if (_dataTemplate == null || !_dataTemplate.TryGetTarget(out var dataTemplate))
            {
                dataTemplate = CreateDataTemplate(_viewProvider, preprocessor);

                _dataTemplate = new WeakReference<DataTemplate>(dataTemplate);
            }

            return dataTemplate;
        }
    }
}
