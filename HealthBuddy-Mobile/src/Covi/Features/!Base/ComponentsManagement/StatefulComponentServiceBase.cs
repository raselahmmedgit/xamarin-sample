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
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

using Covi.Features.ComponentsManagement.Internal;

namespace Covi.Features.ComponentsManagement
{
    /// <summary>
    /// <see cref="IComponentService"/> abstract implementation to create <see cref="IComponent"/> items which state is handled by
    /// the subclass of this service.
    /// </summary>
    /// <typeparam name="TState">State of the component service.
    /// State built by the default constructor of the <see cref="TState"/> should represent initial state of the service.</typeparam>
    public abstract class StatefulComponentServiceBase<TState> : IComponentService
        where TState : new()
    {
        private readonly CompositeDisposable _hostLifecycleSubscription = new CompositeDisposable();
        private readonly CompositeDisposable _currentComponentsDisposable = new CompositeDisposable();

        // BehaviorSubject is used here to allow late HostContext attachment.
        private BehaviorSubject<IComponentsGroup> _subject;

        public StatefulComponentServiceBase()
        {
            var initialState = GetInitialState();
            var initialItems = UpdateState(initialState);
            _subject = new BehaviorSubject<IComponentsGroup>(new ComponentsGroup(ComponentKey, initialItems));
        }

        /// <summary>
        /// Gets the identifier of the component service. Should be equal to the nameof(ComponentServiceType).
        /// </summary>
        public abstract string ComponentKey { get; }

        public IObservable<IComponentsGroup> Items => _subject;

        protected HostContext HostContext { get; private set; }

        /// <summary>
        /// Gets the list of the <see cref="IComponent"/> items from the <paramref name="state"/>.
        /// </summary>
        /// <param name="state">State of the service.</param>
        /// <returns>Collection of the <see cref="IComponent"/> items.</returns>
        protected abstract IList<IComponent> UpdateState(TState state);

        /// <summary>
        /// Should return initial service state.
        /// </summary>
        /// <returns>Initial service state.</returns>
        protected virtual TState GetInitialState()
        {
            return new TState();
        }

        /// <summary>
        /// Sets the current state of the service.
        /// Manages attachment and detachment of the <see cref="IComponent"/> items to the <see cref="HostContext"/>.
        /// </summary>
        /// <param name="state">State of the service.</param>
        protected void SetState(TState state)
        {
            var componentsList = UpdateState(state) ?? new List<IComponent>();
            _currentComponentsDisposable.Clear();
            var itemsGroup = new ComponentsGroup(ComponentKey);
            foreach (var viewModel in componentsList)
            {
                itemsGroup.Add(viewModel);

                if (HostContext != null)
                {
                    viewModel.Attach(HostContext, ComponentKey)
                        .DisposeWith(_currentComponentsDisposable);
                }
            }

            _subject.OnNext(itemsGroup);
        }

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

            _hostLifecycleSubscription.Clear();

            HostContext.ActivatorManager.Initialized
                .Subscribe((_) => OnInitialized())
                .DisposeWith(_hostLifecycleSubscription);

            HostContext.ActivatorManager.Activated
                .Subscribe((_) => OnActivated())
                .DisposeWith(_hostLifecycleSubscription);

            HostContext.ActivatorManager.Deactivated
                .Subscribe((_) => OnDeactivated())
                .DisposeWith(_hostLifecycleSubscription);
        }

        /// <summary>
        /// Called upon component host's initialization.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        /// <summary>
        /// Called upon component host's activation.
        /// </summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>
        /// Called upon component host's deactivation.
        /// </summary>
        protected virtual void OnDeactivated()
        {
        }

        public void Dispose()
        {
            _currentComponentsDisposable.Clear();
            _hostLifecycleSubscription.Clear();
            _subject.Dispose();
        }
    }
}
