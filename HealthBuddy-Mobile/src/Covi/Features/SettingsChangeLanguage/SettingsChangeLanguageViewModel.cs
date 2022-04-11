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
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Covi.Features.Analytics;
using Covi.Features.ChangeLanguage.Components;
using Covi.Features.SettingsChangeLanguage.Actions;
using Covi.Services.Localization;
using Covi.Services.Notification;
using MediatR;

namespace Covi.Features.SettingsChangeLanguage
{
    public class SettingsChangeLanguageViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationManager _notificationManager;

        public SettingsChangeLanguageViewModel(
            IMediator mediator,
            ILocalizationService localizationService,
            INotificationManager notificationManager)
        {
            _mediator = mediator;
            _notificationManager = notificationManager;
            _localizationService = localizationService;
            SelectLanguageViewModel = new SelectLanguageViewModel(localizationService, HandleChangeLanguageAsync);
        }

        public SelectLanguageViewModel SelectLanguageViewModel { get; }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(SettingsChangeLanguageViewModel));
        }

        private async Task HandleChangeLanguageAsync(CultureInfo selectedCulture)
        {
            if (selectedCulture != null)
            {
                await _localizationService.SaveSelectedCultureAsync(selectedCulture);

                var result = await _notificationManager.ShowNotificationAsync(
                    string.Empty,
                    Covi.Features.ChangeLanguage.Resources.Localization.ChangeLanguage_AlertDescription_Text,
                    Covi.Resources.Localization.Dialog_Accept);

                //await _mediator.Send(new AcceptInitialSettingsLanguageAction());
            }
        }
    }
}
