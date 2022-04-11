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

using Covi.Features.Analytics;
using Covi.Features.UserDeleteAccount.Services;
using Covi.Features.Welcome.Routes;
using Covi.Services.ErrorHandlers;
using Covi.Utils;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Covi.Features.UserDeleteAccount
{
    public class DeleteAccountViewModel : ViewModelBase
    {
        private readonly IDeleteAccountService _deleteAccountService;
        private readonly IWelcomeRoute _welcomeRoute;
        private readonly INavigationService _navigationService;
        private readonly IErrorHandler _errorHandler;

        public DeleteAccountViewModel(
            IDeleteAccountService deleteAccountService,
            IWelcomeRoute welcomeRoute,
            INavigationService navigationService,
            IErrorHandler errorHandler)
        {
            _deleteAccountService = deleteAccountService;
            _welcomeRoute = welcomeRoute;
            _navigationService = navigationService;
            _errorHandler = errorHandler;

            IsDeleteDataConfirmationState = true;
            IsCompletedState = false;

            DeleteButtonCommand = ReactiveCommand.CreateFromTask(DeleteUserAsync);
        }

        public ReactiveUI.ReactiveCommand<Unit, Unit> DeleteButtonCommand { get; set; }

        public bool IsDeleteDataConfirmationState { get; private set; }
        public bool IsCompletedState { get; private set; }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(DeleteAccountViewModel));
        }

        private async Task DeleteUserAsync()
        {
            IsBusy = true;
            try
            {
                await _deleteAccountService.DeleteUserAsync();
                IsBusy = false;
                await HandleOperationCompletedAsync();
            }
            catch (Exception ex)
            {
                await _errorHandler.HandleAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task HandleOperationCompletedAsync()
        {
            try
            {
                IsDeleteDataConfirmationState = false;
                IsCompletedState = true;
                await NavigateToWelcomeAsync();
            }
            catch (Exception ex)
            {
                await _errorHandler.HandleAsync(ex);
            }
        }

        private async Task NavigateToWelcomeAsync()
        {
            try
            {
                await Task.Delay(3000);
                await _welcomeRoute.ExecuteAsync(_navigationService);
            }
            catch (Exception ex)
            {
                await _errorHandler.HandleAsync(ex);
            }
        }
    }
}
