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
using System.Collections.Generic;
using System.Linq;

namespace Covi.Features
{
    public class ComponentsManager
    {
        private readonly ConcurrentDictionary<string, IDisposable> _components = new ConcurrentDictionary<string, IDisposable>();

        public void Attach(string key, IDisposable disposable)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var isAdded = _components.TryAdd(key, disposable);
                if (!isAdded)
                {
                    throw new InvalidOperationException($"{nameof(ComponentsManager)}.{nameof(Attach)} failed to attach component with key = {key}. Component with such key was already attached.");
                }
            }
        }

        public void Detach(string key)
        {
            if (!string.IsNullOrEmpty(key) && _components.TryRemove(key, out var disposable))
            {
                disposable?.Dispose();
            }
        }

        public void Clear()
        {
            var itemsToClear = _components.Values.ToList();
            _components.Clear();

            foreach (var disposable in itemsToClear)
            {
                disposable?.Dispose();
            }
        }
    }
}
