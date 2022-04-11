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

using Covi.Features.Analytics;
using Covi.Features.Filters.Dialogs;
using Covi.Features.Newsfeed.Services;
using Covi.Services.Dialogs;
using Covi.Services.ErrorHandlers;
using Covi.Services.Navigation;
using Covi.Utils;
using DynamicData;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IDialogService = Covi.Services.Dialogs.IDialogService;

namespace Covi.Features.Filters
{
    public class FiltersViewModel : ViewModelBase
    {
        private readonly IErrorHandler _errorHandler;
        private readonly IDialogService _dialogService;
        private readonly INewsfeedService _newsfeedService;
        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        private readonly SourceList<FilterDescriptionViewModel> _models = new SourceList<FilterDescriptionViewModel>();

        private readonly ReadOnlyObservableCollection<FilterDescriptionViewModel> _filters;
        public ReadOnlyObservableCollection<FilterDescriptionViewModel> Filters => _filters;

        public FiltersViewModel(
            IErrorHandler errorHandler,
            IDialogService dialogService,
            INewsfeedService newsfeedService,
            INavigationServiceDelegate navigationServiceDelegate)
        {
            _errorHandler = errorHandler;
            _dialogService = dialogService;
            _newsfeedService = newsfeedService;
            _navigationServiceDelegate = navigationServiceDelegate;

            _models.Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _filters, 1)
                .Subscribe();

            SaveFiltersCommand = ReactiveCommand.CreateFromTask(SaveFiltersAsync);
            ResetToDefaultsFiltersCommand = ReactiveCommand.CreateFromTask(ResetToDefaultsAsync);
        }

        public ReactiveCommand<Unit, Unit> SaveFiltersCommand { get; }

        public ReactiveCommand<Unit, Unit> ResetToDefaultsFiltersCommand { get; }

        private async void SelectionHandler(FilterDescriptionViewModel model)
        {
            await ShowFilterDialogAsync(model);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await InitializeFiltersAsync();
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(FiltersViewModel));
        }

        private async Task InitializeFiltersAsync()
        {
            try
            {
                IsBusy = true;

                await ResetFilterChangesAsync();
            }
            catch (Exception e)
            {
                await _errorHandler.HandleAsync(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateDisplayedFilters(IEnumerable<FilterDescription> filters)
        {
            _models.Edit((innerList) =>
            {
                filters ??= Enumerable.Empty<FilterDescription>();

                var viewModels = filters.Select(item => new FilterDescriptionViewModel(item, SelectionHandler)).ToList();

                var lastItem = viewModels.LastOrDefault();
                if (lastItem != null)
                {
                    lastItem.IsLast = true;
                }

                innerList.Clear();
                innerList.AddRange(viewModels);
            });
        }

        public async Task ShowFilterDialogAsync(FilterDescriptionViewModel model)
        {
            var parameters =
                new FilterOptionDialogRequestParameters(model.DisplayName, model.IsMultiSelect, model.FilterOptionsModels);

            await _dialogService.ShowDialogAsync(nameof(FilterOptionsDialog), parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters == null)
            {
                return;
            }

            await HandleDialogResultAsync(parameters);
        }

        private async Task HandleDialogResultAsync(INavigationParameters dialogResult)
        {
            var result = await dialogResult.FromDialogNavigationParametersAsync<FilterOptionDialogResponseParameters>();

            if (result?.FilterOptions == null)
            {
                return;
            }

            IList<FilterDescription> currentFilters = new List<FilterDescription>();
            foreach (var filter in result.FilterOptions)
            {
                currentFilters = _newsfeedService.SetFilterOption(filter);
            }

            UpdateDisplayedFilters(currentFilters);
        }

        private async Task HandleDialogResultAsync(Prism.Services.Dialogs.IDialogResult dialogResult)
        {
            var result = await dialogResult.Parameters.FromDialogParametersAsync<FilterOptionDialogResponseParameters>();

            if (result?.FilterOptions == null)
            {
                return;
            }

            IList<FilterDescription> currentFilters = new List<FilterDescription>();
            foreach (var filter in result.FilterOptions)
            {
                currentFilters = _newsfeedService.SetFilterOption(filter);
            }

            UpdateDisplayedFilters(currentFilters);
        }

        private async Task ResetFilterChangesAsync()
        {
            var items = await _newsfeedService.ResetToLastSavedAsync();
            UpdateDisplayedFilters(items);
        }

        private async Task ResetToDefaultsAsync()
        {
            try
            {
                var items = await _newsfeedService.ResetToDefaultsAsync();
                UpdateDisplayedFilters(items);
            }
            catch (Exception e)
            {
                await _errorHandler.HandleAsync(e);
            }
        }

        private async Task SaveFiltersAsync()
        {
            await _newsfeedService.CommitFilterChangesAsync();
            await _navigationServiceDelegate.GoBackAsync();
        }
    }
}
