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
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Covi.Features
{
    public class ViewModelActivatorManager : IDisposable
    {
        private readonly List<Func<IEnumerable<IDisposable>>> _blocks;
        private IDisposable _activationHandle = Disposable.Empty;

        private readonly Subject<Unit> _activated;
        private readonly Subject<Unit> _deactivated;
        private readonly Subject<Unit> _initialized;

        private readonly BehaviorSubject<ViewModelState> _state;
        private int _refCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelActivatorManager"/> class.
        /// </summary>
        public ViewModelActivatorManager()
        {
            _activated = new Subject<Unit>();
            _deactivated = new Subject<Unit>();
            _initialized = new Subject<Unit>();
            _state = new BehaviorSubject<ViewModelState>(ViewModelState.NotInitialized);
            _blocks = new List<Func<IEnumerable<IDisposable>>>();
        }

        public IObservable<Unit> Activated => _activated;

        public IObservable<Unit> Deactivated => _deactivated;

        public IObservable<Unit> Initialized => _initialized;

        public IObservable<ViewModelState> State => _state;

        public bool IsDisposed { get; private set; }

        public void Initialize()
        {
            OnNextInitialized();
            OnNextState(ViewModelState.Initialized);
        }

        /// <summary>
        /// This method is called by the framework when the corresponding View
        /// is activated. Call this method in unit tests to simulate a ViewModel
        /// being activated.
        /// </summary>
        /// <returns>A Disposable that calls Deactivate when disposed.</returns>
        public IDisposable Activate()
        {
            if (Interlocked.Increment(ref _refCount) == 1)
            {
                var disp = new CompositeDisposable(_blocks.SelectMany(x => x()));
                Interlocked.Exchange(ref _activationHandle, disp).Dispose();
                OnNextActivated();
            }

            var result = Disposable.Create(() => Deactivate());
            OnNextState(ViewModelState.Activated);

            return result;
        }

        /// <summary>
        /// This method is called by the framework when the corresponding View
        /// is deactivated.
        /// </summary>
        /// <param name="ignoreRefCount">
        /// Force the VM to be deactivated, even
        /// if more than one person called Activate.
        /// </param>
        public void Deactivate(bool ignoreRefCount = false)
        {
            if (Interlocked.Decrement(ref _refCount) == 0 || ignoreRefCount)
            {
                Interlocked.Exchange(ref _activationHandle, Disposable.Empty).Dispose();
                OnNextDeactivated();
            }

            OnNextState(ViewModelState.Deactivated);
        }

        public IDisposable AttachChildActivatorManager(ViewModelActivatorManager childActivatorManager)
        {
            var disposable = new CompositeDisposable();
            Initialized.Subscribe(_ => childActivatorManager.Initialize())
                .DisposeWith(disposable);
            Activated.Subscribe(_ => childActivatorManager.Activate())
                .DisposeWith(disposable);
            Deactivated.Subscribe(_ => childActivatorManager.Deactivate())
                .DisposeWith(disposable);
            State.Take(1).Subscribe(state => SetComponentActivationState(state, childActivatorManager));
            return disposable;
        }

        private void SetComponentActivationState(ViewModelState hostState, ViewModelActivatorManager childActivatorManager)
        {
            // ViewModel.Initialize method should be called explicitly and only once. It should be done from the child view model side.
            switch (hostState)
            {
                case ViewModelState.Activated:
                    childActivatorManager.Activate();
                    break;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            _activated.Dispose();
            _deactivated.Dispose();
            _initialized.Dispose();
            _state.Dispose();
            _activationHandle?.Dispose();

            IsDisposed = true;
        }

        private void OnNextActivated()
        {
            if (!IsDisposed)
            {
                _activated.OnNext(Unit.Default);
            }
        }

        private void OnNextDeactivated()
        {

            if (!IsDisposed)
            {
                _deactivated.OnNext(Unit.Default);
            }
        }

        private void OnNextInitialized()
        {
            if (!IsDisposed)
            {
                _initialized.OnNext(Unit.Default);
            }
        }

        private void OnNextState(ViewModelState state)
        {
            if (!IsDisposed)
            {
                _state.OnNext(state);
            }
        }

        /// <summary>
        /// Adds a action blocks to the list of registered blocks. These will called
        /// on activation, then disposed on deactivation.
        /// </summary>
        /// <param name="block">The block to add.</param>
        internal void AddActivationBlock(Func<IEnumerable<IDisposable>> block)
        {
            _blocks.Add(block);
        }
    }
}
