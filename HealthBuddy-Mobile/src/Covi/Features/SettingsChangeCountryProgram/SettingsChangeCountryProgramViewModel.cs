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

using System.Globalization;
using System.Threading.Tasks;
using Covi.Features.SettingsChangeCountryProgram.Actions;
using Covi.Features.ChangeCountryProgram.Components;
using Covi.Features.FirebaseRemoteConfig;
using Covi.Services.Localization;
using MediatR;
using Covi.Features.ChangeCountryProgram;
using Covi.Services.Notification;
using Covi.Features.Welcome.Routes;
using Prism.Navigation;
using Covi.Features.Main.Routes;

namespace Covi.Features.SettingsChangeCountryProgram
{
    public class SettingsChangeCountryProgramViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        //private readonly ILocalizationService _localizationService;
        private readonly IFirebaseRemoteConfigurationService _iFirebaseRemoteConfigurationService;
        private readonly INotificationManager _notificationManager;

        //public SettingsChangeCountryProgramViewModel(
        //    IMediator mediator,
        //    ILocalizationService localizationService)
        //{
        //    _mediator = mediator;
        //    _localizationService = localizationService;
        //    SelectCountryProgramViewModel = new SelectCountryProgramViewModel(localizationService, HandleSettingsChangeCountryProgramAsync);
        //}

        public SettingsChangeCountryProgramViewModel(
            IMediator mediator,
            IFirebaseRemoteConfigurationService iFirebaseRemoteConfigurationService,
            INotificationManager notificationManager)
        {
            _mediator = mediator;
            _iFirebaseRemoteConfigurationService = iFirebaseRemoteConfigurationService;
            _notificationManager = notificationManager;
            SelectCountryProgramViewModel = new SelectCountryProgramViewModel(_iFirebaseRemoteConfigurationService, HandleSettingsChangeCountryProgramAsync);
        }

        //public SettingsChangeCountryProgramViewModel(
        //    IMediator mediator)
        //{
        //    _mediator = mediator;
        //    SelectCountryProgramViewModel = new SelectCountryProgramViewModel(HandleSettingsChangeCountryProgramAsync);
        //}

        public SelectCountryProgramViewModel SelectCountryProgramViewModel { get; }

        //private async Task HandleSettingsChangeCountryProgramAsync(CultureInfo selectedCulture)
        //{
        //    if (selectedCulture != null)
        //    {
        //        await _localizationService.SaveSelectedCultureAsync(selectedCulture);

        //        _localizationService.InitializeLocale();
        //        await _mediator.Send(new AcceptInitialCountryProgramAction());
        //    }
        //}

        private async Task HandleSettingsChangeCountryProgramAsync(CountryProgramFirebaseChannel selectedFirebaseChannel)
        {
            if (selectedFirebaseChannel != null)
            {
                //var result = await _notificationManager.ShowNotificationAsync(
                //    string.Empty,
                //    Covi.Features.ChangeCountryProgram.Resources.Localization.ChangeCountryProgram_AlertDescription_Text,
                //    Covi.Resources.Localization.Dialog_Accept);

                await _mediator.Send(new AcceptInitialSettingsCountryProgramAction());
            }
        }
    }
}
