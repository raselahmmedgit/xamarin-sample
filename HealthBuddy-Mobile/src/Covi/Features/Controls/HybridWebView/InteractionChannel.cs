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
using Covi.Features.Chat.Handlers;
using Covi.Services.Serialization;
using ReactiveUI;

namespace Covi.Features.Controls.HybridWebView
{
    /// <summary>
    /// The interaction channel between a hybrid webview host and a messages handler.
    /// The handler is able to send messages via SendMessageInteraction whereas the host uses HandleMessage method of the channel.
    /// </summary>
    public class InteractionChannel
    {
        private readonly IInteractionChannelHost _host;
        private IInteractionChannelHandler _handler;
        private IDisposable _handlerSubscription;

        public InteractionChannel(IInteractionChannelHost host)
        {
            _host = host;
        }

        public void SetHandler(IInteractionChannelHandler handler)
        {
            _handler = handler;
            _handlerSubscription?.Dispose();
            _handlerSubscription = _handler?.SendMessageInteraction?.RegisterHandler(SendMessage);
        }

        private async Task SendMessage(InteractionContext<Message, bool> context)
        {
            await SendMessageAsync(context.Input);

            context.SetOutput(true);
        }

        /// <summary>
        /// Extracts the message from the string and sends it to the handler if there is one.
        /// </summary>
        /// <param name="data">Serialized message.</param>
        public async void HandleMessage(string data)
        {
            var message = await TryDecodeMessageAsync(data).ConfigureAwait(false);
            if (message != null)
            {
                _handler?.HandleMessageCommand?.Execute(message);
            }
        }

        private async Task SendMessageAsync(Message message)
        {
            var messageString = await EncodeMessageAsync(message).ConfigureAwait(false);
            await _host.SendMessageAsync(messageString);
        }

        private async Task<T> InvokeAsync<T>(Func<T> function)
        {
            if (Xamarin.Essentials.MainThread.IsMainThread)
                return function();
            else
                return await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(function).ConfigureAwait(false);
        }

        private async Task<Message> TryDecodeMessageAsync(string serializedMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serializedMessage) || serializedMessage == "undefined")
                {
                    return null;
                }
                var message = await Serializer.Instance.DeserializeAsync<Message>(serializedMessage).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(message?.Payload))
                {
                    //Payload is a base64 string. Decode into json string and replace the initial one.
                    var decodedPayload = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(message.Payload));
                    message.Payload = decodedPayload;
                }

                return message;
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> EncodeMessageAsync(Message message)
        {
            if (message == null)
            {
                return null;
            }

            var encodedPayload = string.IsNullOrWhiteSpace(message.Payload)
                ? null
                : System.Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(message.Payload));
            var msg = new Message
            {
                EventName = message.EventName,
                Payload = encodedPayload
            };
            var serialisedResult = await Serializer.Instance.SerializeAsync(msg).ConfigureAwait(false);
            return serialisedResult;
        }
    }
}
