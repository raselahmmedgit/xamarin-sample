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
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Prism.Navigation;

namespace Covi.Features
{
    public class ComponentViewModelBase : ViewModelBase, IComponent
    {
        public string ComponentGroupKey { get; private set; }

        protected INavigationService Navigation { get; private set; }

        public IDisposable Attach(HostContext hostContext, string componentGroupKey)
        {
            Navigation = hostContext.NavigationService;
            ComponentGroupKey = componentGroupKey;

            var detachDisposable = new CompositeDisposable();

            hostContext.ActivatorManager.State.Take(1).Subscribe(SetComponentInitializationState);
            hostContext.ActivatorManager.AttachChildActivatorManager(HostContext.ActivatorManager).DisposeWith(detachDisposable);

            detachDisposable.Add(Disposable.Create(OnDeactivated));

            return detachDisposable;
        }

        private void SetComponentInitializationState(ViewModelState hostState)
        {
            if (hostState == ViewModelState.NotInitialized)
            {
                return;
            }

            switch (hostState)
            {
                case ViewModelState.Initialized:
                case ViewModelState.Deactivated:
                case ViewModelState.Activated:
                    Initialize(null);
                    break;
            }
        }

    }
}
