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
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Covi.Features.Account;
using Covi.Features.Account.Services.SignIn;
using Covi.Features.Account.Services.SignOut;
using Covi.Features.Analytics;
using Covi.Features.Chat.Data;
using Covi.Features.OnBoarding.Routes;
using Covi.Services.ErrorHandlers;
using Covi.Utils;
using MediatR;

using Prism.Navigation;
using ReactiveUI;

using Unit = System.Reactive.Unit;

namespace Covi.Features.UserLogIn
{
    public class UserLogInViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private string _userName;
        private string _userPassword;
        private readonly IErrorHandler _errorHandler;
        private readonly ISignInService _singInService;
        private readonly ISignOutService _signOutService;

        private readonly ChatDatabase _chatDatabase;

        public ReactiveCommand<Unit, Unit> LogInCommand { get; }

        public UserLogInViewModel(
            IMediator mediator,
            IErrorHandler errorHandler,
            ISignInService singInService,
            ISignOutService signOutService)
        {
            _mediator = mediator;
            _errorHandler = errorHandler;
            _singInService = singInService;
            _signOutService = signOutService;

            var canExecuteLogin = this.WhenAnyValue(
                vm => vm.HasUsernameErrorMessage,
                vm => vm.HasPasswordErrorMessage,
                vm => vm.IsBusy,
                ValidateCanExecuteLogin);

            _chatDatabase = new ChatDatabase();

            LogInCommand = ReactiveCommand.CreateFromTask(HandleLogInAsync, canExecuteLogin);
        }

        private static bool ValidateCanExecuteLogin(bool? hasUserNameError, bool? hasPasswordError, bool isBusy)
        {
            bool isUserNameInvalid = hasUserNameError.HasValue && hasUserNameError.Value;
            bool isPasswordInvalid = hasPasswordError.HasValue && hasPasswordError.Value;

            return !isUserNameInvalid && !isPasswordInvalid && !isBusy;
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                ValidateUsername(value);
                this.RaisePropertyChanged();
            }
        }

        public string UserPassword
        {
            get => _userPassword;
            set
            {
                _userPassword = value;
                ValidatePassword(value);
                this.RaisePropertyChanged();
            }
        }

        private void ValidateUsername(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                HasUsernameErrorMessage = true;
                UsernameErrorText = UserLogIn.Resources.Localization.UserLogIn_Username_EmptyErrorMessage_Text;
            }
            else
            {
                HasUsernameErrorMessage = false;
                UsernameErrorText = string.Empty;
            }
        }

        private void ValidatePassword(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                HasPasswordErrorMessage = true;
                PasswordErrorText = UserLogIn.Resources.Localization.UserLogIn_Password_EmptyErrorMessage_Text;
            }
            else
            {
                HasPasswordErrorMessage = false;
                PasswordErrorText = string.Empty;
            }
        }

        public bool? HasUsernameErrorMessage { get; private set; }

        public bool? HasPasswordErrorMessage { get; private set; }

        public string UsernameErrorText { get; set; }

        public string PasswordErrorText { get; set; }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(UserLogInViewModel));
        }

        private async Task HandleLogInAsync()
        {
            try
            {
                if (HasPasswordErrorMessage.HasValue && HasPasswordErrorMessage.Value == false &&
                    HasUsernameErrorMessage.HasValue && HasUsernameErrorMessage.Value == false)
                {
                    IsBusy = true;
                    await _signOutService.SignOutAsync();
                    await _singInService.SignInAsync(
                        new UserCredentials(UserName, UserPassword));
                    await _chatDatabase.DeleteAllRapidProMessageAsync();

                    Xamarin.Essentials.Preferences.Set("SignedIn", true);
                    await _mediator.Send(new UserLogInFinishedAction());
                }
            }
            catch (LoginOperationException e)
            {
                PasswordErrorText = e.Message;
            }
            catch (Exception e)
            {
                await _errorHandler.HandleAsync(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
