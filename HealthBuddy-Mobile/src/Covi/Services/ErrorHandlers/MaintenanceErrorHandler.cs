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
using Covi.Features.Maintenance;
using Covi.Services.Dispatcher;
using Covi.Services.Http.ApiExceptions;
using Covi.Services.Navigation;
using Prism.Navigation;

namespace Covi.Services.ErrorHandlers
{
    public class MaintenanceErrorHandler : IErrorHandler
    {
        private readonly INavigationServiceDelegate _navigationServiceDelegate;
        private readonly IDispatcherService _dispatcherService;

        public MaintenanceErrorHandler(INavigationServiceDelegate navigationServiceDelegate, IDispatcherService dispatcherService)
        {
            _navigationServiceDelegate = navigationServiceDelegate;
            _dispatcherService = dispatcherService;
        }

        public async Task<bool> HandleAsync(Exception error)
        {
            if (error is MaintenanceResponseException maintenanceResponseException)
            {
                var message = maintenanceResponseException.ErrorMessage;
                var page = $"/{nameof(Features.Shell.ShellPage)}/{nameof(MaintenancePage)}";
                var errorMessageParameters = new ErrorMessageParameters()
                {
                    ErrorMessage = string.IsNullOrEmpty(message) ? Features.Maintenance.Resources.Localization.MaintenanceDescriptionTitle_Text : message
                };
                NavigationParameters navigationParameters = await errorMessageParameters.ToNavigationParametersAsync();
                await _dispatcherService.InvokeAsync(async () => await _navigationServiceDelegate.NavigateAsync(page, navigationParameters));
                return true;
            }
            return false;
        }
    }
}
