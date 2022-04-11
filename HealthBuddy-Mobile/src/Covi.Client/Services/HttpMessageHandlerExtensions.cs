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
using System.Net.Http;

namespace Covi.Client.Services
{
    public static class HttpMessageHandlerExtensions
    {
        public static HttpMessageHandler AddHandlersChain(this HttpMessageHandler clientHandler, params DelegatingHandler[] handlers)
        {
            HttpMessageHandler handler = clientHandler;
            if (handlers != null)
            {
                for (int handlerIndex = handlers.Length - 1; handlerIndex >= 0; handlerIndex--)
                {
                    DelegatingHandler val2 = handlers[handlerIndex];
                    while (val2.InnerHandler is DelegatingHandler)
                    {
                        val2 = (val2.InnerHandler as DelegatingHandler);
                    }

                    val2.InnerHandler = handler;
                    handler = handlers[handlerIndex];
                }
            }

            return handler;
        }
    }
}
