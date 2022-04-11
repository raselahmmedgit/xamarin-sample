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
using Covi.Client.Services.Platform.Models;
using Covi.Features.Account.Models;
using Covi.Features.Account.Services;
using Covi.Features.UserProfile.Services;
using Covi.Features.UserState.Services;
using Covi.Services.ApplicationMetadata;

namespace Covi.Features.UserData.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserAccountContainer _userAccountContainer;
        private readonly IUserStatusContainer _userStatusContainer;
        private readonly IMetadataService _metadataService;
        private readonly IAccountInformationContainer _accountInformationContainer;

        public UserDataService(
            IUserAccountContainer userAccountContainer,
            IUserStatusContainer userStatusContainer,
            IMetadataService metadataService,
            IAccountInformationContainer accountInformationContainer)
        {
            _userAccountContainer = userAccountContainer;
            _userStatusContainer = userStatusContainer;
            _metadataService = metadataService;
            _accountInformationContainer = accountInformationContainer;
        }

        public async Task SetUserDataAsync(UserDataInfo userDataInfo)
        {
            await _metadataService.SetMetadataAsync(userDataInfo?.Metadata).ConfigureAwait(false);
            await _userAccountContainer.SetAsync(userDataInfo.UserAccountInfo).ConfigureAwait(false);
            await _userStatusContainer.SetAsync(userDataInfo?.UserStatus).ConfigureAwait(false);

            var accountInformation = userDataInfo?.UserAccountInfo?.UserAccount?.Roles != null ?
                new AccountInformation(userDataInfo.UserAccountInfo.UserAccount.Roles)
                : null;
            await _accountInformationContainer.SetAsync(accountInformation).ConfigureAwait(false);
        }
    }
}
