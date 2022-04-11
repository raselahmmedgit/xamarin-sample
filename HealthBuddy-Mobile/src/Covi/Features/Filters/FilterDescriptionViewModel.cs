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

using Covi.Features.Filters.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Covi.Features.Filters
{
    public class FilterDescriptionViewModel
    {
        private readonly FilterDescription _item;
        private readonly Action<FilterDescriptionViewModel> _selectionHandler;

        public FilterDescriptionViewModel(FilterDescription item, Action<FilterDescriptionViewModel> selectionHandler)
        {
            _item = item;
            _selectionHandler = selectionHandler;
        }

        public string ParameterName => _item.ParameterName;

        public string DisplayName => _item.DisplayName;

        public List<FilterOptionItem> FilterOptionsModels => _item.Values;

        public bool IsMultiSelect => _item.MultiSelect;

        public bool IsLast { get; set; }

        public string SelectedDisplayValue
        {
            get
            {
                var result = string.Empty;

                var selectedItems = _item.Values.Where(item => item.IsChecked).Select(s => s.DisplayName).ToList();

                if (selectedItems.Count() == _item.Values.Count)
                {
                    result = Resources.Localization.Filter_AllSelected_Text;
                }
                else if (selectedItems.Count() == 0)
                {
                    result = Resources.Localization.Filter_NoneSelected_Text;
                }
                else
                {
                    selectedItems.ForEach(i => result = string.Join(", ", selectedItems));
                }

                return result;
            }
        }

        public ICommand FilterTappedCommand => ReactiveUI.ReactiveCommand.Create(() => _selectionHandler.Invoke(this));
    }
}
