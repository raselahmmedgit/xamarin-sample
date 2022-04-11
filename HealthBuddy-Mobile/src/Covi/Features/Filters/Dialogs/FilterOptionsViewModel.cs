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

using Covi.Features.Components.DialogPage;
using Covi.Services.Dialogs;
using Covi.Services.ErrorHandlers;
using DynamicData;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Covi.Features.Filters.Dialogs
{
    public class FilterOptionsViewModel : DialogViewModelBase
    {
        private readonly IErrorHandler _errorHandler;

        private readonly SourceList<FilterOptionItemViewModel> _models = new SourceList<FilterOptionItemViewModel>();

        private readonly ReadOnlyObservableCollection<FilterOptionItemViewModel> _filterOptions;

        public ReadOnlyObservableCollection<FilterOptionItemViewModel> FilterOptions => _filterOptions;

        public FilterOptionsViewModel(
            IErrorHandler errorHandler,
            ILoggerFactory loggerFactory,
            IDialogService dialogService)
            : base(loggerFactory, dialogService)
        {
            _errorHandler = errorHandler;

            _models.Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _filterOptions, 1)
                .Subscribe();

            SaveAndCloseDialogCommand = ReactiveCommand.CreateFromTask(SaveAsync);

            ReturnToDefaultValuesCommand = ReactiveCommand.CreateFromTask(ReturnToDefaultValuesAsync);
        }

        public string FilterName { get; set; }

        public bool IsMultiSelect { get; private set; }

        public ReactiveCommand<Unit, Unit> SaveAndCloseDialogCommand { get; }
        public ReactiveCommand<Unit, Unit> ReturnToDefaultValuesCommand { get; }

        public override async void OnDialogDataLoaded(INavigationParameters parameters)
        {
            base.OnDialogDataLoaded(parameters);

            var request = await parameters.FromDialogNavigationParametersAsync<FilterOptionDialogRequestParameters>();
            Initialize(request);
        }

        private void Initialize(FilterOptionDialogRequestParameters parameters)
        {
            if (parameters == null)
            {
                return;
            }

            FilterName = parameters.FilterName;
            IsMultiSelect = parameters.IsMultiSelect;
            SetFiltersAsync(parameters.FilterOptions);
        }

        private async Task SaveAsync()
        {
            var parameters = new FilterOptionDialogResponseParameters(_models.Items.Select(i => i.Item).ToList());
            var navigationParameters = await parameters.ToDialogNavigationParametersAsync();
            await CloseDialogAsync(navigationParameters);
        }

        private FilterOptionItemViewModel CreateItemViewModel(FilterOptionItem filterOption)
        {
            return new FilterOptionItemViewModel(filterOption, SelectionHandler, IsMultiSelect);
        }

        private IList<FilterOptionItemViewModel> CreateViewModels(IEnumerable<FilterOptionItem> filterOptions)
        {
            filterOptions = filterOptions ?? Enumerable.Empty<FilterOptionItem>();
            var viewModels = filterOptions.Select(CreateItemViewModel).ToList();

            var lastItem = viewModels.LastOrDefault();
            if (lastItem != null)
            {
                lastItem.IsLast = true;
            }

            return viewModels;
        }

        private Task ReturnToDefaultValuesAsync()
        {
            _models.Edit(list =>
            {
                var editList = list.ToList();
                list.Clear();

                editList.ForEach(item => item.IsChecked = item.IsDefault);

                list.AddRange(editList);
            });

            return Task.FromResult(true);
        }

        private void SelectionHandler(FilterOptionItemViewModel viewModel)
        {
            _models.Items.ForEach(item =>
            {
                if (item.Value != viewModel.Value)
                {
                    return;
                }

                var newItem = new FilterOptionItem
                {
                    DisplayName = viewModel.DisplayName,
                    IsChecked = viewModel.IsChecked,
                    IsDefault = viewModel.IsDefault,
                    Value = viewModel.Value
                };

                if (IsMultiSelect)
                {
                    ReplaceModel(item.Item, newItem);
                }
                else
                {
                    if (viewModel.IsChecked == false)
                    {
                        newItem.IsChecked = true;

                        ReplaceModel(item.Item, newItem,
                            list => list
                                .Where(i => i.Value != viewModel.Value)
                                .ForEach(i => i.IsChecked = false));
                    }
                    else
                    {
                        ReplaceModel(item.Item, newItem,
                            list => list
                                .Where(i => i.Value != viewModel.Value)
                                .ForEach(i => i.IsChecked = false));
                    }
                }
            });
        }

        private void ReplaceModel(
            FilterOptionItem original,
            FilterOptionItem newModel,
            Action<IList<FilterOptionItem>> actionBeforeReplacement = null)
        {
            _models.Edit(list =>
            {
                var editList = list.Select(i => i.Item).ToList();

                actionBeforeReplacement?.Invoke(editList);

                editList.Replace(original, newModel);

                var viewModels = CreateViewModels(editList);

                list.Clear();
                list.AddRange(viewModels);
            });
        }

        private async void SetFiltersAsync(List<FilterOptionItem> filterOptions)
        {
            try
            {
                IsBusy = true;

                _models.Edit(innerList =>
                {
                    innerList.Clear();
                    var viewModels = CreateViewModels(filterOptions);

                    innerList.AddRange(viewModels);
                });
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
    }
}
