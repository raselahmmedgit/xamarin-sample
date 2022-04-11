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

namespace Covi.Services.Platform
{
    public static class HttpHandlersContainer
    {
        private static IHttpHandlersChainBuilder _instance;
        private static readonly Lazy<IHttpHandlersChainBuilder> _defaultInstance =
            new Lazy<IHttpHandlersChainBuilder>(() => new HttpHandlersChainBuilder());

        public static IHttpHandlersChainBuilder Instance
        {
            get { return _instance ??= _defaultInstance.Value; }
        }

        public static void Setup(IHttpHandlersChainBuilder manager)
        {
            _instance = manager;
        }
    }
}
