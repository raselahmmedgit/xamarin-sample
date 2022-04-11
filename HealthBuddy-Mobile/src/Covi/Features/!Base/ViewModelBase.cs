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
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;
using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI;

namespace Covi.Features
{
    public abstract class ViewModelBase : BindableObject, INavigationAware, IConfirmNavigation, IDestructible, IInitialize, IPageLifecycleAware, IActivatableViewModel
    {
        private readonly CompositeDisposable _lifecycleDisposable = new CompositeDisposable();
        private CancellationTokenSource _lifecycleCancellationTokenSource = new CancellationTokenSource();
        private int _busyRefCount;

        public CompositeDisposable LifecycleDisposable => _lifecycleDisposable;

        public CancellationToken LifecycleToken => _lifecycleCancellationTokenSource.Token;

        public CancellationTokenSource LifecycleCancellationTokenSource => _lifecycleCancellationTokenSource;

        public ViewModelBase()
        {
            HostContext = new HostContext();
            this.WhenActivated(disposables =>
            {
                this.OnActivated(disposables);

                Disposable
                    .Create(this.OnDeactivated)
                    .DisposeWith(disposables);
            });
        }

        public HostContext HostContext { get; }

        public bool IsBusy
        {
            get
            {
                var count = Interlocked.CompareExchange(ref _busyRefCount, 0, 0);
                return count > 0;
            }

            protected set
            {
                if (value)
                {
                    Interlocked.Increment(ref _busyRefCount);
                }
                else
                {
                    Interlocked.Decrement(ref _busyRefCount);
                }

                this.RaisePropertyChanged();
            }
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            HostContext.ActivatorManager.Initialize();
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual bool CanNavigate(INavigationParameters parameters)
        {
            return true;
        }

        public virtual void Destroy()
        {
            _lifecycleDisposable.Clear();
            HostContext.ActivatorManager.Dispose();
            Interlocked.Exchange(ref _lifecycleCancellationTokenSource, null)?.Dispose();
        }

        public virtual void OnAppearing()
        {
            Interlocked.Exchange(ref _lifecycleCancellationTokenSource, new CancellationTokenSource())?.Dispose();

            HostContext.ActivatorManager.Activate();
        }

        public virtual void OnActivated(CompositeDisposable lifecycleDisposable)
        {
        }

        public virtual void OnDisappearing()
        {
            _lifecycleDisposable.Clear();
            _lifecycleCancellationTokenSource?.Cancel();
            HostContext.ActivatorManager.Deactivate();
        }

        public virtual void OnDeactivated()
        {
        }
    }
}
