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
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Prism.Ioc;
using Covi.Features.ComponentsManagement.Internal;

namespace Covi.Features.ComponentsManagement
{
    /// <summary>
    /// <see cref="IComponentService"/> abstract implementation to create <see cref="IComponent"/> items which have no state
    /// or which are handling their state by themselves (by relying on other services).
    /// </summary>
    public abstract class StatelessComponentServiceBase : IComponentService
    {
        private readonly IDisposable _subscription;
        private readonly CompositeDisposable _currentComponentsDisposable = new CompositeDisposable();
        private readonly IContainerProvider _containerProvider;

        // BehaviorSubject is used here to allow late HostContext attachment.
        private BehaviorSubject<IComponentsGroup> _subject;

        public StatelessComponentServiceBase(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            _subject = new BehaviorSubject<IComponentsGroup>(new ComponentsGroup(ComponentKey));

            _subscription = GetComponentTypes().Subscribe(SetItems);
        }

        /// <summary>
        /// Gets the identifier of the component service. Should be equal to the nameof(ComponentServiceType).
        /// </summary>
        public abstract string ComponentKey { get; }

        public IObservable<IComponentsGroup> Items => _subject;

        protected HostContext HostContext { get; private set; }

        private void SetItems(IEnumerable<Type> items)
        {
            _currentComponentsDisposable.Clear();
            var itemsGroup = new ComponentsGroup(ComponentKey);

            var typesList = items?.ToList() ?? new List<Type>();
            foreach (var type in typesList)
            {
                var viewModel = _containerProvider.Resolve(type) as IComponent;
                if (viewModel is null)
                {
                    throw new InvalidOperationException($"{nameof(StatelessComponentServiceBase)}.{nameof(GetComponentTypes)}() should return only types of view models with {nameof(IComponent)} interface implemented; {nameof(StatelessComponentServiceBase)}.{nameof(ComponentKey)} = {ComponentKey}");
                }

                itemsGroup.Add(viewModel);

                if (HostContext != null)
                {
                    viewModel.Attach(HostContext, ComponentKey)
                        .DisposeWith(_currentComponentsDisposable);
                }
            }

            _subject.OnNext(itemsGroup);
        }

        /// <summary>
        /// Gets an observable of the list of the component item types. Collection should contain list of types of the components to display at the moment.
        /// <see cref="Type"/> items in the collection must implement <see cref="IComponent"/> interface, otherwise <see cref="InvalidOperationException"/> will be thrown.
        /// </summary>
        /// <returns>An observable list of <see cref="IComponent"/> types.</returns>
        protected abstract IObservable<IList<Type>> GetComponentTypes();

        public void SetHostContext(HostContext hostContext)
        {
            HostContext = hostContext;
            if (_subject.TryGetValue(out var viewModelItems)
                && viewModelItems != null
                && viewModelItems.Any())
            {
                _currentComponentsDisposable.Clear();
                foreach (var viewModel in viewModelItems)
                {
                    viewModel.Attach(hostContext, ComponentKey)
                        .DisposeWith(_currentComponentsDisposable);
                }
            }
        }

        public void Dispose()
        {
            _currentComponentsDisposable.Clear();
            _subscription.Dispose();
            _subject.Dispose();
        }
    }
}
