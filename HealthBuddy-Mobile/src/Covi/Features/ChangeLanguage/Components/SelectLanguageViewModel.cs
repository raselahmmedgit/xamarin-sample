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
using Covi.Services.Localization;
using Covi.Utils.ReactiveCommandHelpers;
using ReactiveUI;
using Xamarin.Forms.Internals;
using Unit = System.Reactive.Unit;

namespace Covi.Features.ChangeLanguage.Components
{
    public class SelectLanguageViewModel : ViewModelBase
    {
        private readonly ILocalizationService _localizationService;
        private readonly Func<CultureInfo, Task> _selectionHandler;

        public IList<LanguageItemViewModel> ProvidedLanguages { get; private set; }

        public ReactiveCommand<Unit, Unit> ChangeLanguageCommand { get; }

        public SelectLanguageViewModel(
            ILocalizationService localizationService,
            Func<CultureInfo, Task> selectionHandler)
        {
            _localizationService = localizationService;
            _selectionHandler = selectionHandler;


            ChangeLanguageCommand = ReactiveCommandFactory.CreateLockedCommand(HandleChangeLanguageAsync);

            Initialize();
        }

        private void Initialize()
        {
            var supportedLocales = _localizationService.AvailableCultures;
            ProvidedLanguages = CreateViewModels(supportedLocales);
        }

        private IList<LanguageItemViewModel> CreateViewModels(IEnumerable<CultureInfo> languageOptions)
        {
            languageOptions = languageOptions ?? Enumerable.Empty<CultureInfo>();
            var viewModels = languageOptions.Select(CreateItemViewModel).ToList();

            if (!viewModels.Any(vm => vm.IsChecked))
            {
                // If no current culture selected, select the first in the list of languages.
                viewModels.FirstOrDefault()?.SetChecked(true);
            }

            var lastItem = viewModels.LastOrDefault();
            if (lastItem != null)
            {
                lastItem.IsLast = true;
            }

            return viewModels;
        }

        private LanguageItemViewModel CreateItemViewModel(CultureInfo culture)
        {
            var item = new LanguageItemViewModel(culture, SelectionHandler);

            if (culture.Equals(_localizationService.CurrentCulture))
            {
                item.SetChecked(true);
            }

            return item;
        }

        private void SelectionHandler(LanguageItemViewModel viewModel)
        {
            ProvidedLanguages.ForEach(m => m.SetChecked(false));
            viewModel.SetChecked(true);
        }

        private async Task HandleChangeLanguageAsync()
        {
            try
            {
                var selectedItem = ProvidedLanguages.FirstOrDefault(x => x.IsChecked)?.Item;

                await _selectionHandler(selectedItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SelectLanguageViewModel - HandleChangeLanguageAsync: Exception - {ex.Message.ToString()}");
            }
        }
    }
}
