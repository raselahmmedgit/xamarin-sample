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

using System.Collections.Generic;
using System.Linq;
using Covi.Features.ComponentsManagement;
using Prism.Navigation;

namespace Covi.Features
{
    public class CompositeViewModelBase : ViewModelBase
    {
        public CompositeViewModelBase(INavigationService navigationService)
        {
            // HostContext should have additional initialized properties in case if it is CompositeViewModelBase.
            HostContext.SetNavigationService(navigationService);
            HostContext.CreateComponentsManager();
        }

        public void AttachToHost(IComponent component, string key, bool shouldReplace = false)
        {
            if (component != null && !string.IsNullOrEmpty(key))
            {
                var disposable = component.Attach(HostContext, key);

                if (shouldReplace)
                {
                    HostContext.ComponentsManager.Detach(key);
                }

                HostContext.ComponentsManager.Attach(key, disposable);
            }
        }

        public void Detach(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                HostContext.ComponentsManager.Detach(key);
            }
        }

        public override void Destroy()
        {
            HostContext.ComponentsManager.Clear();
            base.Destroy();
        }
    }
}
