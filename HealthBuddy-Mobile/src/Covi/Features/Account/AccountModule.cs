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

using Covi.Features.Account.Services;
using Covi.Features.Account.Services.Authentication;
using Covi.Features.Account.Services.SignIn;
using Covi.Features.Account.Services.SignOut;
using Covi.Features.Account.Services.SignUp;
using Covi.Features.UserData.Services;
using Covi.Features.UserState.Services;

using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Features.Account
{
    public class AccountModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAccountInformationContainer, AccountInformationContainer>();
            containerRegistry.RegisterSingleton<IUserStatusContainer, UserStatusContainer>();
            containerRegistry.RegisterSingleton<IAuthenticationServiceErrorHandler, AuthenticationServiceErrorHandler>();
            containerRegistry.Register<IUserDataService, UserDataService>();
            containerRegistry.Register<IAuthenticationInfoService, AuthenticationInfoService>();
            containerRegistry.Register<ISignInService, SignInService>();
            containerRegistry.Register<ISignOutService, SignOutService>();
            containerRegistry.Register<ISignUpService, SignUpService>();
        }
    }
}
