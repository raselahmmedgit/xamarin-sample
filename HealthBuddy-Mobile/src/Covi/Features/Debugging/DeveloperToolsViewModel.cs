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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Covi.Services.Security.SecretsProvider;
using DynamicData;
using Prism.Navigation;
using ReactiveUI;

namespace Covi.Features.Debugging
{
    public class DeveloperToolsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<TraceViewModel> _devicesCollection = new ObservableCollection<TraceViewModel>();

        public ReactiveCommand<Unit, Unit> RestartBluetoothCommand { get; }

        public ReactiveCommand<Unit, Unit> AddContactDeviceCommand { get; }

        public ReactiveCommand<Unit, Unit> RefreshContactedDevicesCommand { get; }

        public ReactiveCommand<Unit, Unit> CleanContactedDevicesStorageCommand { get; }

        public ReadOnlyObservableCollection<TraceViewModel> ContactedDevices { get; }

        public DeveloperToolsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            RestartBluetoothCommand = ReactiveCommand.CreateFromTask(RestartBluetoothAsync);
            AddContactDeviceCommand = ReactiveCommand.CreateFromTask(AddContactDeviceAsync);
            RefreshContactedDevicesCommand = ReactiveCommand.CreateFromTask(RefreshContactedDevicesAsync);
            CleanContactedDevicesStorageCommand = ReactiveCommand.CreateFromTask(CleanContactedDevicesStorageAsync);
            ContactedDevices = new ReadOnlyObservableCollection<TraceViewModel>(_devicesCollection);
        }

        public string DeviceId { get; private set; }

        public string ContactDeviceId { get; set; }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitializeAsync().FireAndForget();
        }

        private async Task InitializeAsync()
        {
            var deviceId = await SecretsProvider.Instance.GetDeviceIdentifierAsync();
            DeviceId = deviceId;
            await RefreshContactedDevicesAsync();
        }

        private Task AddContactDeviceAsync()
        {
            return Task.CompletedTask;
        }

        private Task RestartBluetoothAsync()
        {
            return Task.CompletedTask;
        }

        private Task RefreshContactedDevicesAsync()
        {
            return Task.CompletedTask;
        }

        private Task CleanContactedDevicesStorageAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class TraceViewModel
    {
        public string ContactTimestamp { get; set; }

        public string ContactToken { get; set; }
    }
}
