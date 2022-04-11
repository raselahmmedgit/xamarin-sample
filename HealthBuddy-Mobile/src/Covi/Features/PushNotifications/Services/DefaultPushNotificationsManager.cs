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
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Features.PushNotifications.Services
{
    internal class DefaultPushNotificationsManager : IPushNotificationsManager
    {
        private readonly ConcurrentBag<IPushNotificationHandler> _handlers = new ConcurrentBag<IPushNotificationHandler>();

        public void AddHandler(IPushNotificationHandler handler)
        {
            if (handler != null)
            {
                _handlers.Add(handler);
            }
        }

        public async Task<bool> HandleAsync(PushNotification pushNotification)
        {
            var handlers = _handlers.ToList();
            var result = false;

            foreach (var handler in handlers)
            {
                try
                {
                    result = await handler.HandleAsync(pushNotification);
                    if (result)
                    {
                        break;
                    }
                }
                catch
                {
                    // Log push notification handling errors.
                    result = false;
                }
            }

            return result;
        }
    }
}
