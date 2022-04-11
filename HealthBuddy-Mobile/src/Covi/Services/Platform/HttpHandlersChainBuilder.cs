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
using System.Net.Http;

namespace Covi.Services.Platform
{
    public class HttpHandlersChainBuilder : IHttpHandlersChainBuilder
    {
        private const string GenericHandlersKey = "generic";

        private Dictionary<string, List<Func<DelegatingHandler>>> _chainHandlers;

        public HttpHandlersChainBuilder()
        {
            _chainHandlers = new Dictionary<string, List<Func<DelegatingHandler>>>();
        }

        public void AddHandler(Func<DelegatingHandler> handler)
        {
            AddScopedHandler(GenericHandlersKey, handler);
        }

        public void AddScopedHandler(string scope, Func<DelegatingHandler> handler)
        {
            if (_chainHandlers.ContainsKey(scope))
            {
                _chainHandlers[scope].Add(handler);
            }
            else
            {
                _chainHandlers[scope] = new List<Func<DelegatingHandler>> { handler };
            }
        }

        public IEnumerable<DelegatingHandler> GetHandlers(string scope = null)
        {
            var result = new List<Func<DelegatingHandler>>();

            AddHandlersOfScope(result, GenericHandlersKey);
            if (!string.IsNullOrEmpty(scope))
            {
                AddHandlersOfScope(result, scope);
            }

            var resultHandlers = result.Select(f => f());

            return resultHandlers;
        }

        private void AddHandlersOfScope(List<Func<DelegatingHandler>> collectionToUpdate, string scope)
        {
            if (_chainHandlers.ContainsKey(scope) && (_chainHandlers[scope]?.Any() ?? false))
            {
                collectionToUpdate.AddRange(_chainHandlers[scope]);
            }
        }
    }
}
