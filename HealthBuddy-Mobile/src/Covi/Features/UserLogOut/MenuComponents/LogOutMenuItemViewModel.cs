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

using Covi.Features.Account.Services.SignOut;
using Covi.Features.Chat.Data;
using Covi.Features.Menu;
using Covi.Features.Welcome.Routes;
using Covi.Services;
using Covi.Services.ErrorHandlers;
using Covi.Services.Notification;

using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace Covi.Features.UserLogOut.MenuComponents
{
    public class LogOutMenuItemViewModel : MenuItemViewModel
    {
        private const string LogoutIconSource = "resource://Covi.Features.UserLogOut.Resources.Images.logout.svg";

        private readonly IErrorHandler _errorHandler;
        private readonly IWelcomeRoute _welcomeRoute;
        private readonly INavigationService _navigationService;
        private readonly ISignOutService _signOutService;
        private readonly INotificationManager _notificationManager;
        private readonly ICloseApplication _closeApplication;

        private readonly ChatDatabase _chatDatabase;

        public LogOutMenuItemViewModel(
            IErrorHandler errorHandler,
            INavigationService navigationService,
            ISignOutService signOutService,
            IWelcomeRoute welcomeRoute,
            INotificationManager notificationManager,
            ICloseApplication closeApplication,
            bool isAnonymous)
        {
            _errorHandler = errorHandler;
            _welcomeRoute = welcomeRoute;
            _signOutService = signOutService;
            _navigationService = navigationService;
            _notificationManager = notificationManager;
            _closeApplication = closeApplication;

            _chatDatabase = new ChatDatabase();

            IsAnonymous = isAnonymous;
            Title = isAnonymous ? Resources.Localization.Menu_Exit_ItemText : Resources.Localization.Menu_LogOut_ItemText;
            IconSource = LogoutIconSource;
            NavigateCommand = ReactiveUI.ReactiveCommand.CreateFromTask(GoToLogOutAsync);
        }

        private bool IsAnonymous { get; set; }

        protected async Task GoToLogOutAsync()
        {
            try
            {
                await ShowSignOutDialogAsync();
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

        public async Task ShowSignOutDialogAsync()
        {
            var title = IsAnonymous ? Resources.Localization.Menu_Exit_Dialog_TitleText : Resources.Localization.Menu_LogOut_Dialog_TitleText;
            var description = IsAnonymous ? Resources.Localization.Menu_Exit_Dialog_QuestionText : Resources.Localization.Menu_LogOut_Dialog_QuestionText;

            var result = await _notificationManager.ShowNotificationAsync(
                    title,
                    description,
                    Resources.Localization.Menu_LogOut_Dialog_AcceptBtn,
                    Resources.Localization.Menu_LogOut_Dialog_CancelBtn);

            if (result)
            {
                IsBusy = true;

                await _chatDatabase.DeleteAllRapidProMessageAsync();
                await _signOutService.SignOutAsync();
                //await _welcomeRoute.ExecuteAsync(_navigationService);

                _closeApplication.Close();

                IsBusy = false;
            }
        }

    }
}
