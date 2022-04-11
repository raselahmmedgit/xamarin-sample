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
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Essentials;

namespace Covi.Droid.Features.PushNotifications
{
    public class PushNotificationPermissions : Permissions.BasePlatformPermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            var status = PermissionStatus.Disabled;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var manager = (NotificationManager)Android.App.Application.Context.GetSystemService(Context.NotificationService);
                if (manager != null)
                {
                    if (manager.AreNotificationsEnabled())
                    {
                        status = PermissionStatus.Granted;
                    }
                }
            }
            else
            {
                var enabled = NotificationManagerCompat.From(Android.App.Application.Context)?.AreNotificationsEnabled();
                if (enabled.HasValue && enabled.Value)
                {
                    status = PermissionStatus.Granted;
                }
            }

            return Task.FromResult(status);
        }
    }
}
