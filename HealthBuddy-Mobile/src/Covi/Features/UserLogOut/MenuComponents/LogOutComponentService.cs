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

using Covi.Features.Account.Services.Authentication;
using Covi.Features.Account.Services.SignOut;
using Covi.Features.ComponentsManagement;
using Covi.Features.Welcome.Routes;
using Covi.Services;
using Covi.Services.ErrorHandlers;
using Covi.Services.Notification;

using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covi.Features.UserLogOut.MenuComponents
{
    public class LogOutComponentService : StatefulComponentServiceBase<LogOutState>
    {
        public override string ComponentKey => nameof(LogOutComponentService);

        private readonly IErrorHandler _errorHandler;
        private readonly IWelcomeRoute _welcomeRoute;
        private readonly INavigationService _navigationService;
        private readonly ISignOutService _signOutService;
        private readonly INotificationManager _notificationManager;
        private readonly ICloseApplication _closeApplication;

        public LogOutComponentService(
            IErrorHandler errorHandler,
            INavigationService navigationService,
            ISignOutService signOutService,
            IWelcomeRoute welcomeRoute,
            IAuthenticationInfoService authenticationInfoService,
            INotificationManager notificationManager,
            ICloseApplication closeApplication
            )
        {
            _errorHandler = errorHandler;
            _welcomeRoute = welcomeRoute;
            _signOutService = signOutService;
            _navigationService = navigationService;
            _notificationManager = notificationManager;
            _closeApplication = closeApplication;

            IsAnonymous = authenticationInfoService.IsAnonymous();
        }

        private bool IsAnonymous { get; set; }

        protected override IList<IComponent> UpdateState(LogOutState state)
        {
            if (state == null)
            {
                return new List<IComponent>();
            }

            var result = new List<IComponent>();

            switch (state.State)
            {
                case LogOutContentState.LoggedIn:
                    result.AddRange(GetMenuItems(state));
                    break;
                case LogOutContentState.Default:
                    break;
            }

            return result;
        }

        private IList<IComponent> GetMenuItems(LogOutState state)
        {
            var result = new List<IComponent>();
            foreach (var menuItem in state.MenuItemsList)
            {
                result.Add(menuItem);
            }

            return result;
        }

        protected override async void OnActivated()
        {
            base.OnActivated();

            var menuItemsList = await GetLogOutItemsListAsync();

            SetState(LogOutState.CreateMenuState(menuItemsList));
        }

        private Task<IList<LogOutMenuItemViewModel>> GetLogOutItemsListAsync()
        {


            IList<LogOutMenuItemViewModel> items = new List<LogOutMenuItemViewModel>
            {
                new LogOutMenuItemViewModel(
                    _errorHandler,
                    _navigationService,
                    _signOutService,
                    _welcomeRoute,
                    _notificationManager,
                    _closeApplication,
                    IsAnonymous)
            };

            return Task.FromResult(items);
        }
    }
}
