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
using Android.Content;

namespace Covi.Droid.Features.PushNotifications
{
    public class PushNotificationPermissionInitializer : Covi.Features.PushNotifications.IPushNotificationInitializer
    {
        public async Task<bool> InitializeAsync()
        {
            var isPermisionGranted = await IsPermissionGrantedAsync();

            if (!isPermisionGranted)
            {
                OpenAppSettings();
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> IsPermissionGrantedAsync()
        {
            var result = await Xamarin.Essentials.Permissions.CheckStatusAsync<PushNotificationPermissions>();
            return result == Xamarin.Essentials.PermissionStatus.Granted;
        }

        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            var uri = Android.Net.Uri.FromParts("package", Android.App.Application.Context.PackageName, null);
            intent.SetData(uri);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}
