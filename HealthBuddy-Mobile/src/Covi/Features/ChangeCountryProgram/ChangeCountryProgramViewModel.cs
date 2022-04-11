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
using System.Windows.Input;
using Covi.Features.ChangeCountryProgram.Actions;
using Covi.Features.ChangeCountryProgram.Components;
using Covi.Features.ChangeLanguage.Routes;
using Covi.Features.FirebaseRemoteConfig;
using Covi.Services.Localization;
using Covi.Services.Navigation;
using MediatR;
using Xamarin.Forms;

namespace Covi.Features.ChangeCountryProgram
{
    public class ChangeCountryProgramViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        //private readonly ILocalizationService _localizationService;
        private readonly IFirebaseRemoteConfigurationService _iFirebaseRemoteConfigurationService;

        private readonly IChangeLanguageRoute _changeLanguageRoute;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        public ICommand OnBackChangeLanguageCommand { get; set; }

        //public ChangeCountryProgramViewModel(
        //    IMediator mediator,
        //    ILocalizationService localizationService)
        //{
        //    _mediator = mediator;
        //    _localizationService = localizationService;
        //    SelectCountryProgramViewModel = new SelectCountryProgramViewModel(localizationService, HandleChangeCountryProgramAsync);
        //}

        public ChangeCountryProgramViewModel(
            IMediator mediator,
            IFirebaseRemoteConfigurationService iFirebaseRemoteConfigurationService,
            IChangeLanguageRoute changeLanguageRoute,
            INavigationServiceDelegate navigationServiceDelegate)
        {
            _mediator = mediator;
            _iFirebaseRemoteConfigurationService = iFirebaseRemoteConfigurationService;
            _changeLanguageRoute = changeLanguageRoute;
            _navigationServiceDelegate = navigationServiceDelegate;

            SelectCountryProgramViewModel = new SelectCountryProgramViewModel(_iFirebaseRemoteConfigurationService, HandleChangeCountryProgramAsync);

            OnBackChangeLanguageCommand = new Command(() =>
            {
                _changeLanguageRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
            });
        }

        //public ChangeCountryProgramViewModel(
        //    IMediator mediator)
        //{
        //    _mediator = mediator;
        //    SelectCountryProgramViewModel = new SelectCountryProgramViewModel(HandleChangeCountryProgramAsync);
        //}

        public SelectCountryProgramViewModel SelectCountryProgramViewModel { get; }

        //private async Task HandleChangeCountryProgramAsync(CultureInfo selectedCulture)
        //{
        //    if (selectedCulture != null)
        //    {
        //        await _localizationService.SaveSelectedCultureAsync(selectedCulture);

        //        _localizationService.InitializeLocale();
        //        await _mediator.Send(new AcceptInitialCountryProgramAction());
        //    }
        //}

        private async Task HandleChangeCountryProgramAsync(CountryProgramFirebaseChannel selectedFirebaseChannel)
        {
            if (selectedFirebaseChannel != null)
            {
                await _mediator.Send(new AcceptInitialCountryProgramAction());
            }
        }
    }
}
