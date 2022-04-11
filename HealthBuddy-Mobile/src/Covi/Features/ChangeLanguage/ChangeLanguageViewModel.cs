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
using Covi.Features.ChangeLanguage.Actions;
using Covi.Features.ChangeLanguage.Components;
using Covi.Services.Localization;
using MediatR;

namespace Covi.Features.ChangeLanguage
{
    public class ChangeLanguageViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;

        public ChangeLanguageViewModel(
            IMediator mediator,
            ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            SelectLanguageViewModel = new SelectLanguageViewModel(localizationService, HandleChangeLanguageAsync);
        }

        public SelectLanguageViewModel SelectLanguageViewModel { get; }

        private async Task HandleChangeLanguageAsync(CultureInfo selectedCulture)
        {
            if (selectedCulture != null)
            {
                await _localizationService.SaveSelectedCultureAsync(selectedCulture);

                _localizationService.InitializeLocale();
                await _mediator.Send(new AcceptInitialLanguageAction());
            }
        }
    }
}
