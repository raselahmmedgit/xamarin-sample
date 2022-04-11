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

using ReactiveUI;
using Covi.Features.PushNotifications.Services;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Covi.Features.PushNotifications.SettingsComponents
{
    public class NotificationsItemViewModel : ComponentViewModelBase
    {
        private readonly IPushNotificationsService _pushNotificationsService;

        private bool _permissionGranted;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    if (value && !_permissionGranted)
                    {
                        Task.Run(() =>
                        {
                            _pushNotificationsService.InitializeAsync();
                        });
                    }

                    if (!value)
                    {
                        _pushNotificationsService.OpenAppSettings();
                    }
                }

                this.RaiseAndSetIfChanged(ref _isEnabled, value);
            }
        }

        public NotificationsItemViewModel(IPushNotificationsService pushNotificationsService)
        {
            _pushNotificationsService = pushNotificationsService;
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);
            SetPermissionsFields();
        }

        private async void SetPermissionsFields()
        {
            _permissionGranted = await _pushNotificationsService.IsPermissionGrantedAsync();
            IsEnabled = _permissionGranted;
        }
    }
}
