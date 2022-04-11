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
using System.Threading.Tasks;
using Covi.Services.Serialization;
using Microsoft.Extensions.Logging;

namespace Covi.Features.Chat.Handlers
{
    public class MessagesProcessor : IMessagesProcessor
    {
        private readonly ISerializer _serializer;
        private readonly List<IMessageHandler> _innerHandlers = new List<IMessageHandler>();
        private readonly ILogger _logger;

        public MessagesProcessor(
            ISerializer serializer,
            ILoggerFactory loggerFactory,
            params IMessageHandler[] handlers)
        {
            _serializer = serializer;
            _logger = loggerFactory.CreateLogger<MessagesProcessor>();
            _innerHandlers.AddRange(handlers);
        }

        public async Task HandleAsync(Message message)
        {
            if (message == null)
            {
                return;
            }

            try
            {
                foreach (var handler in _innerHandlers)
                {
                    if (await handler.HandleAsync(message))
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
               _logger.LogError(e, "Handling the message from js failed.");
            }
        }

        public void AddHandler(IMessageHandler handler)
        {
            if (handler != null)
            {
                _innerHandlers.Add(handler);
            }
        }

        public void RemoveHandler(IMessageHandler handler)
        {
            if (handler != null)
            {
                _innerHandlers.Remove(handler);
            }
        }
    }
}
