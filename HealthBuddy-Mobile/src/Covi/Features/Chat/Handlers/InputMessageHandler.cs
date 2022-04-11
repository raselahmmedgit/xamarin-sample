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
using System.Threading.Tasks;

namespace Covi.Features.Chat.Handlers
{
    public class InputMessageHandler : IMessageHandler
    {
        private const string ShowInputEventName = "showInput";
        private const string HideInputEventName = "hideInput";
        private readonly Action<bool> _changeInputVisibility;

        public InputMessageHandler(Action<bool> changeInputVisibility)
        {
            _changeInputVisibility = changeInputVisibility;
        }

        public async Task<bool> HandleAsync(Message message)
        {
            switch (message.EventName)
            {
                case ShowInputEventName:
                    await ShowInputAsync();
                    return true;
                case HideInputEventName:
                    await HideInputAsync();
                    return true;
                default:
                    return false;
            }
        }

        private async Task HideInputAsync()
        {
            _changeInputVisibility?.Invoke(false);
            await Task.CompletedTask;
        }

        private async Task ShowInputAsync()
        {
            _changeInputVisibility?.Invoke(true);
            await Task.CompletedTask;
        }
    }
}
