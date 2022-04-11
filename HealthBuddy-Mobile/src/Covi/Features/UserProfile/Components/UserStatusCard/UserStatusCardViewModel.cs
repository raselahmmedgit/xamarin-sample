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

using Covi.Client.Services.Platform.Models;
using Covi.Features.UserProfile.Services;
using Covi.Services.ApplicationMetadata;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.ComponentModel;
using Covi.Features.UserState.Services;

namespace Covi.Features.UserProfile.Components.UserStatusCard
{
    public class UserStatusCardViewModel : ComponentViewModelBase
    {
        private UserStatus _userStatus;
        private readonly IUserAccountContainer _userProfileContainer;
        private readonly IMetadataContainer _metadataContainer;

        public string HealthStatus { get; private set; }
        public string StatusCountdown { get; private set; }
        public string UserName { get; private set; }
        public Severity Severity { get; set; }

        public UserStatusCardViewModel(
            IUserAccountContainer userProfileContainer,
            IUserStatusContainer userStatusContainer,
            IMetadataContainer metadataContainer)
        {
            _userProfileContainer = userProfileContainer;
            _metadataContainer = metadataContainer;
            this.WhenActivated((d) =>
            {
                userStatusContainer.Changes
                    .DistinctUntilChanged(new UserStateStatusEqualityComparer())
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(HandleProfileModelChanged)
                    .DisposeWith(d);
            });
        }



        private void HandleProfileModelChanged(UserStatus profile)
        {
            SetProfile(profile);
        }

        private async void SetProfile(UserStatus userStatus)
        {
            if (userStatus != null)
            {
                var metadata = await _metadataContainer.GetAsync();
                var userAccount = await _userProfileContainer.GetAsync();

                if (metadata != null && !userAccount.IsAnonymous)
                {
                    _userStatus = userStatus;
                    UserName = userAccount.UserAccount.Username;
                    var status = metadata.Metadata.Statuses.Values.FirstOrDefault(x => x.Id == _userStatus.StatusId);
                    if (status != null)
                    {
                        Severity = status.Severity != null && Enum.IsDefined(typeof(Severity), status.Severity)
                                       ? (Severity)status.Severity
                                       : Severity.NotRecognized;

                        HealthStatus = status.Name;
                        if (_userStatus.StatusChangedOn.HasValue)
                        {
                            var daysOfStatus =
                                Math.Round((DateTimeOffset.UtcNow - _userStatus.StatusChangedOn.Value).TotalDays);
                            StatusCountdown = string.Format(
                                Resources.Localization.UserStatus_StatusCountdown_TextFormat,
                                HealthStatus, daysOfStatus);
                        }
                    }
                    else
                    {
                        StatusCountdown = string.Empty;
                        HealthStatus = string.Empty;
                    }
                }
            }
        }

        private class UserStateStatusEqualityComparer : IEqualityComparer<UserStatus>
        {
            public bool Equals(UserStatus x, UserStatus y)
            {
                if (x == null && y == null)
                    return true;
                else if (x == null || y == null)
                    return false;
                return x.StatusId == y.StatusId;
            }

            public int GetHashCode(UserStatus obj)
            {
                return obj.StatusId.GetHashCode();
            }
        }
    }

    [DefaultValue(NotRecognized)]
    public enum Severity
    {
        NotRecognized = -1,
        Low = 0,
        High = 1,
        Critical = 2
    }
}
