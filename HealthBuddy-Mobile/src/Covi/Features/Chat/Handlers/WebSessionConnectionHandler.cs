using System;
using System.Threading.Tasks;
using Covi.Services.Serialization;
using Microsoft.Extensions.Logging;

namespace Covi.Features.Chat.Handlers
{
    public class WebSessionConnectionHandler : IMessageHandler
    {
        private const string EventName = "operationResult";
        private const string SuccessStatus = "success";
        private const string ErrorStatus = "error";
        private readonly Action<bool> _connectionActionHandler;
        private readonly ILogger _logger;

        public WebSessionConnectionHandler(
            ILoggerFactory loggerFactory,
            Action<bool> changeConnectionStatus)
        {
            _logger = loggerFactory.CreateLogger<WebSessionConnectionHandler>();
            _connectionActionHandler = changeConnectionStatus;
        }

        private bool IsConnected { get; set; } = true;

        public async Task<bool> HandleAsync(Message message)
        {
            var result = string.Equals(message.EventName, EventName);
            if (result)
            {
                try
                {
                    var statusPayload = await Serializer.Instance.DeserializeAsync<StatusPayload>(message.Payload).ConfigureAwait(false);
                    switch (statusPayload.Status)
                    {
                        case SuccessStatus:
                            if (!IsConnected)
                            {
                                IsConnected = true;
                                await ConnectionSuccessAsync();
                            }

                            break;
                        case ErrorStatus:
                            if (IsConnected)
                            {
                                IsConnected = false;
                                await ConnectionErrorAsync();
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Web view connection status handling failed.");
                }
            }

            return result;
        }

        private async Task ConnectionSuccessAsync()
        {
            _connectionActionHandler?.Invoke(true);
            await Task.CompletedTask;
        }

        private async Task ConnectionErrorAsync()
        {
            _connectionActionHandler?.Invoke(false);
            await Task.CompletedTask;
        }
    }
}
