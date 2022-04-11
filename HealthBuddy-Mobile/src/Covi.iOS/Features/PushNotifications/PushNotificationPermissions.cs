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

using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;

namespace Covi.iOS.Features.PushNotifications
{
    public class PushNotificationPermissions : Permissions.BasePlatformPermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            var currentNotificationType = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types;
            return Task.FromResult(GetNotificationStatus(currentNotificationType));
        }

        private static PermissionStatus GetNotificationStatus(UIUserNotificationType type)
        {
            PermissionStatus result;

            switch (type)
            {
                case UIUserNotificationType.None:
                    result = PermissionStatus.Restricted;
                    break;
                case UIUserNotificationType.Alert:
                case UIUserNotificationType.Sound:
                case UIUserNotificationType.Alert|UIUserNotificationType.Sound:
                    result = PermissionStatus.Granted;
                    break;
                default:
                    result = PermissionStatus.Unknown;
                    break;
            }

            return result;
        }
    }
}
