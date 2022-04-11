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

using Covi.Logs;
using Covi.Services.Dispatcher;
using Covi.Services.Navigation;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Covi.Services.Dialogs
{
    public class DialogService : IDialogService
    {
        private readonly Prism.Services.Dialogs.IDialogService _dialogService;
        private readonly IDispatcherService _dispatcherService;
        private readonly ILogger _logger;
        private readonly INavigationServiceDelegate _navigationService;

        public DialogService(
            Prism.Services.Dialogs.IDialogService dialogService,
            IDispatcherService dispatcherService,
            INavigationServiceDelegate navigationServiceDelegate,
            ILoggerFactory loggerFactory)
        {
            _dialogService = dialogService;
            _dispatcherService = dispatcherService;
            _navigationService = navigationServiceDelegate;
            _logger = loggerFactory.CreateLogger<DialogService>();
        }

        public Task<INavigationResult> ShowDialogAsync<TRequest>(string dialogName, TRequest request)
            where TRequest : DialogParametersBase
        {
            var tcs = new TaskCompletionSource<INavigationResult>();
            Task.Factory.StartNew(async () =>
            {
                var parameters = await request.ToDialogNavigationParametersAsync();

                try
                {
                    await _dispatcherService.InvokeAsync(async () => await _navigationService.NavigateAsync(dialogName, parameters, useModalNavigation: true, false));
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    _logger.LogErrorExceptCancellation(ex, $"Failed to show dialog page {dialogName}.");
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        public Task<Prism.Services.Dialogs.IDialogResult> ShowPrismDialogAsync<TRequest>(string dialogName, TRequest request)
            where TRequest : DialogParametersBase
        {
            var tcs = new TaskCompletionSource<Prism.Services.Dialogs.IDialogResult>();
            Task.Factory.StartNew(async () =>
            {
                var parameters = await request.ToDialogParametersAsync();

                try
                {
                    await _dispatcherService.InvokeAsync(() => _dialogService.ShowDialog(dialogName, parameters, (result) => tcs.TrySetResult(result)));
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    _logger.LogErrorExceptCancellation(ex, $"Failed to show dialog page {dialogName}.");
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }


        public async Task CloseDialogAsync<TResult>(TResult resultParams)
            where TResult : INavigationParameters
        {
            try
            {
                await _dispatcherService.InvokeAsync(async () => await _navigationService.GoBackAsync(resultParams, useModalNavigation: true, false));
            }
            catch (Exception ex)
            {
                Debugger.Break();
                _logger.LogErrorExceptCancellation(ex, $"Failed to close dialog page.");
            }
        }

        public async Task CloseDialogAsync()
        {
            try
            {
                await _dispatcherService.InvokeAsync(async () => await _navigationService.GoBackAsync(useModalNavigation: true, animated: false));
            }
            catch (Exception ex)
            {
                Debugger.Break();
                _logger.LogErrorExceptCancellation(ex, $"Failed to close dialog page.");
            }
        }
    }
}
