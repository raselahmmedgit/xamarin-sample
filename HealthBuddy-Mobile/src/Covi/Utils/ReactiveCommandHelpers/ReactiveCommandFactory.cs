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
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Covi.Utils.ReactiveCommandHelpers
{
    public static class ReactiveCommandFactory
    {
        private static ConcurrentDictionary<string, WeakReference<BehaviorSubject<bool>>> _scopedObservables = new ConcurrentDictionary<string, WeakReference<BehaviorSubject<bool>>>();

        private static BehaviorSubject<bool> GetOrCreateCanExecuteObservable(string scope)
        {
            BehaviorSubject<bool> canExecuteSubject = null;
            if (!string.IsNullOrEmpty(scope))
            {
                if (!(_scopedObservables.TryGetValue(scope, out var subjectReference)
                    && subjectReference.TryGetTarget(out canExecuteSubject)))
                {
                    // If there is no record or the reference is dead, create new observable.
                    canExecuteSubject = new BehaviorSubject<bool>(true);
                    var reference = new WeakReference<BehaviorSubject<bool>>(canExecuteSubject);
                    _scopedObservables.AddOrUpdate(scope, reference, (_, __) => reference);
                }
            }

            canExecuteSubject ??= new BehaviorSubject<bool>(true);

            return canExecuteSubject;
        }

        public static ReactiveCommand<Unit, Unit> CreateLockedCommand(Func<Task> execute, string scope = null)
        {
            var canExecuteSubject = GetOrCreateCanExecuteObservable(scope);

            return ReactiveCommand.CreateFromTask(
                () => ExecuteLockedCommandAsync(execute, canExecuteSubject),
                canExecuteSubject);
        }

        public static ReactiveCommand<TParam, Unit> CreateLockedCommand<TParam>(Func<TParam, Task> execute, string scope = null)
        {
            var canExecuteSubject = GetOrCreateCanExecuteObservable(scope);

            return ReactiveCommand.CreateFromTask<TParam>(
                (param) => ExecuteLockedCommandAsync(execute, param, canExecuteSubject),
                canExecuteSubject);
        }

        public static ReactiveCommand<TParam, TResult> CreateLockedCommand<TParam, TResult>(Func<TParam, Task<TResult>> execute, string scope = null)
        {
            var canExecuteSubject = GetOrCreateCanExecuteObservable(scope);

            return ReactiveCommand.CreateFromTask<TParam, TResult>(
                (param) => ExecuteLockedCommandAsync(execute, param, canExecuteSubject),
                canExecuteSubject);
        }

        private static async Task ExecuteLockedCommandAsync(Func<Task> task, BehaviorSubject<bool> canExecuteSubject)
        {
            try
            {
                canExecuteSubject.OnNext(false);
                await task();
            }
            finally
            {
                canExecuteSubject.OnNext(true);
            }
        }

        private static async Task ExecuteLockedCommandAsync<TParam>(Func<TParam, Task> task, TParam parameter, BehaviorSubject<bool> canExecuteSubject)
        {
            try
            {
                canExecuteSubject.OnNext(false);
                await task(parameter);
            }
            finally
            {
                canExecuteSubject.OnNext(true);
            }
        }

        private static async Task<TResult> ExecuteLockedCommandAsync<TParam, TResult>(Func<TParam, Task<TResult>> task, TParam parameter, BehaviorSubject<bool> canExecuteSubject)
        {
            try
            {
                canExecuteSubject.OnNext(false);
                var result = await task(parameter);
                return result;
            }
            finally
            {
                canExecuteSubject.OnNext(true);
            }
        }
    }
}
