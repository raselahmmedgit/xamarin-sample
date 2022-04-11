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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Covi.Features.FirebaseRemoteConfig;
using Covi.Features.RapidProFcmPushNotifications.Services;
using Covi.Services.Localization;
using Covi.Utils.ReactiveCommandHelpers;
using ReactiveUI;
using Xamarin.Forms.Internals;
using Unit = System.Reactive.Unit;

namespace Covi.Features.ChangeCountryProgram.Components
{
    public class SelectCountryProgramViewModel : ViewModelBase
    {
        //private readonly ILocalizationService _localizationService;
        private readonly Func<CountryProgramFirebaseChannel, Task> _selectionHandler;
        private readonly FirebaseContainer _firebaseContainer;

        //public IList<CountryProgramItemViewModel> ProvidedCountryPrograms { get; private set; }
        public List<CountryProgramItemViewModel> CountryProgramItemViewModels { get; set; }
        public CountryProgramItemViewModel SelectedCountryProgramItemViewModel { get; set; }

        public ReactiveCommand<Unit, Unit> ChangeCountryProgramCommand { get; }

        public string FirebaseRemoteConfigurationInitPhrase = "health_buddy_channel";
        private Func<CountryProgramFirebaseChannel, Task> handleChangeCountryProgramAsync;
        private readonly IFirebaseRemoteConfigurationService _iFirebaseRemoteConfigurationService;

        public bool IsEnable { get; set; }

        public SelectCountryProgramViewModel(
            //ILocalizationService localizationService,
            IFirebaseRemoteConfigurationService iFirebaseRemoteConfigurationService,
            Func<CountryProgramFirebaseChannel, Task> selectionHandler)
        {
            //_localizationService = localizationService;
            _iFirebaseRemoteConfigurationService = iFirebaseRemoteConfigurationService;
            _selectionHandler = selectionHandler;
            _firebaseContainer = new FirebaseContainer();

            ChangeCountryProgramCommand = ReactiveCommandFactory.CreateLockedCommand(HandleChangeCountryProgramAsync);

            InitializeAsync();
        }

        //public SelectCountryProgramViewModel(
        //    //ILocalizationService localizationService,
        //    Func<CountryProgramFirebaseChannel, Task> selectionHandler)
        //{
        //    //_localizationService = localizationService;
        //    _selectionHandler = selectionHandler;
        //    _firebaseContainer = new FirebaseContainer();

        //    ChangeCountryProgramCommand = ReactiveCommandFactory.CreateLockedCommand(HandleChangeCountryProgramAsync);

        //    InitializeAsync();
        //}

        private async void InitializeAsync()
        {
            IsBusy = true;
            IsEnable = false;
            await _iFirebaseRemoteConfigurationService.FetchAndActivateAsync();
            var firebaseRemoteConfigurations = await _iFirebaseRemoteConfigurationService.GetAllAsync<FirebaseRemoteConfiguration>(FirebaseRemoteConfigurationInitPhrase);

            var countryProgramFirebaseChannels = new List<CountryProgramFirebaseChannel>();
            if (firebaseRemoteConfigurations.Any())
            {
                firebaseRemoteConfigurations.Take(5).ForEach(x => {
                    var countryProgramFirebaseChannel = new CountryProgramFirebaseChannel() { ChannelName = x.ChannelName, ChannelId = x.ChannelId, ChannelHost = x.ChannelHost, IsActive = Convert.ToBoolean(x.IsActive) };
                    countryProgramFirebaseChannels.Add(countryProgramFirebaseChannel);
                });
            }
            var countryProgramItemViewModels = CreateViewModels(countryProgramFirebaseChannels);
            CountryProgramItemViewModels = countryProgramItemViewModels.ToList();
            if (!string.IsNullOrEmpty(_firebaseContainer.FirebaseChannelId))
            {
                SelectedCountryProgramItemViewModel = CountryProgramItemViewModels.FirstOrDefault(x => x.Item.ChannelId == _firebaseContainer.FirebaseChannelId);
            }
            else
            {
                SelectedCountryProgramItemViewModel = CountryProgramItemViewModels.FirstOrDefault();
            }
            IsBusy = false;
            IsEnable = true;
        }

        private IList<CountryProgramItemViewModel> CreateViewModels(IEnumerable<CountryProgramFirebaseChannel> countryProgramOptions)
        {
            countryProgramOptions = countryProgramOptions ?? Enumerable.Empty<CountryProgramFirebaseChannel>();
            var viewModels = countryProgramOptions.Select(CreateItemViewModel).ToList();

            if (!viewModels.Any(vm => vm.IsChecked))
            {
                // If no current culture selected, select the first in the list of countryPrograms.
                viewModels.FirstOrDefault()?.SetChecked(true);
            }

            var lastItem = viewModels.LastOrDefault();
            if (lastItem != null)
            {
                lastItem.IsLast = true;
            }

            return viewModels;
        }

        private CountryProgramItemViewModel CreateItemViewModel(CountryProgramFirebaseChannel firebaseChannel)
        {
            var item = new CountryProgramItemViewModel(firebaseChannel, SelectionHandler);

            //if (firebaseChannel.Equals(_localizationService.CurrentCulture))
            //{
            //    item.SetChecked(true);
            //}
            
            if (!string.IsNullOrEmpty(_firebaseContainer.FirebaseChannelId))
            {
                if (item.Value.Equals(_firebaseContainer.FirebaseChannelId))
                {
                    item.SetChecked(true);
                }
            }

            return item;
        }

        private void SelectionHandler(CountryProgramItemViewModel viewModel)
        {
            //ProvidedCountryPrograms.ForEach(m => m.SetChecked(false));
            viewModel.SetChecked(true);
        }

        private async Task HandleChangeCountryProgramAsync()
        {
            try
            {
                //var selectedItem = ProvidedCountryPrograms.FirstOrDefault(x => x.IsChecked)?.Item;
                var selectedItem = SelectedCountryProgramItemViewModel.Item;
                if (selectedItem != null)
                {
                    _firebaseContainer.FirebaseChannelHost = selectedItem.ChannelHost;
                    _firebaseContainer.FirebaseChannelId = selectedItem.ChannelId;

                    await _selectionHandler(selectedItem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SelectCountryProgramViewModel - HandleChangeCountryProgramAsync: Exception - {ex.Message.ToString()}");
            }
        }

    }
}
