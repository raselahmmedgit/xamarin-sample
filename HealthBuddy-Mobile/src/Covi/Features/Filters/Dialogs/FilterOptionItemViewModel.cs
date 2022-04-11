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
using System.Windows.Input;
using ReactiveUI;

namespace Covi.Features.Filters.Dialogs
{
    public class FilterOptionItemViewModel : ViewModelBase
    {
        private readonly Action<FilterOptionItemViewModel> _selectionHandler;

        public FilterOptionItemViewModel(FilterOptionItem item, Action<FilterOptionItemViewModel> selectionHandler, bool multiSelect)
        {
            Item = item;
            _isChecked = item.IsChecked;
            _selectionHandler = selectionHandler;
            IsMultiSelect = multiSelect;
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref _isChecked, value);
                Item.IsChecked = value;
                _selectionHandler.Invoke(this);
            }
        }

        public FilterOptionItem Item { get; }

        public bool IsMultiSelect { get; }

        public bool IsDefault => Item.IsDefault;

        public string DisplayName => Item.DisplayName;

        public string Value => Item.Value;

        public bool IsLast { get; set; }

        public ICommand FilterTappedCommand => ReactiveUI.ReactiveCommand.Create(FilterTapped);

        public void FilterTapped()
        {
            _selectionHandler.Invoke(this);
        }
    }
}
