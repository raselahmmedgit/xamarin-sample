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
using System.Globalization;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Covi.Features.UserProfile.Services;
using Covi.Services.Serialization;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Covi.Features.Chat.Handlers
{
    public class MessageInteractor : IMessageInteractor
    {
        private const string InitEventName = "init";
        private const string SendTextEventName = "sendText";
        private const string ResetEventName = "reset";

        private readonly ISerializer _serializer;
        private readonly Interaction<Message, bool> _sendInteraction;
        private readonly IUserAccountContainer _userAccountContainer;
        private ILogger _logger;

        public MessageInteractor(
            ILoggerFactory loggerFactory,
            ISerializer serializer,
            IUserAccountContainer userAccountContainer,
            Interaction<Message, bool> sendInteraction)
        {
            _logger = loggerFactory.CreateLogger<MessageInteractor>();
            _serializer = serializer;
            _userAccountContainer = userAccountContainer;
            _sendInteraction = sendInteraction;
        }

        public async Task<bool> SendInit(string initPhrase, string initPrefix = null)
        {
            var result = false;
            try
            {
                var userAccount = await _userAccountContainer.GetAsync().ConfigureAwait(false);
                var sessionId = userAccount?.UserAccount?.UserId != null ? $"{initPrefix ?? initPhrase}_{userAccount.UserAccount.UserId}" : null;
                var payload = new InitPayload
                {
                    InitPhrase = initPhrase,
                    Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                    UserId = sessionId
                };
                var serializedPayload = await _serializer.SerializeAsync(payload).ConfigureAwait(false);
                var message = new Message
                {
                    EventName = InitEventName,
                    Payload = serializedPayload
                };

                result = await _sendInteraction.Handle(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure during chat initialization.");
            }

            return result;
        }

        public async Task<bool> SendText(string text)
        {
            var result = false;
            try
            {
                var payload = new SendTextPayload
                {
                    Text = text
                };
                var serializedPayload = await _serializer.SerializeAsync(payload).ConfigureAwait(false);
                var message = new Message
                {
                    EventName = SendTextEventName,
                    Payload = serializedPayload
                };

                result = await _sendInteraction.Handle(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Send message operation failed.");
            }

            return result;
        }

        public async Task<bool> Reset()
        {
            var result = false;
            try
            {
                var message = new Message
                {
                    EventName = ResetEventName
                };

                result = await _sendInteraction.Handle(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure during chat reset.");
            }

            return result;
        }
    }
}
