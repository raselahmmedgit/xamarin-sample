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
using System.Globalization;
using System.Windows.Input;
using ReactiveUI;

namespace Covi.Features.ChangeLanguage
{
    public class LanguageItemViewModel : ViewModelBase
    {
        private readonly Action<LanguageItemViewModel> _selectionHandler;
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                SetChecked(value);
                _selectionHandler.Invoke(this);
            }
        }

        public string DisplayName { get; }

        public string Value => Item.TwoLetterISOLanguageName;

        public CultureInfo Item { get; }

        public bool IsLast { get; set; }

        public ICommand SelectCommand => ReactiveCommand.Create(OnSelected);

        public LanguageItemViewModel(CultureInfo item, Action<LanguageItemViewModel> selectionHandler)
        {
            Item = item;
            if (!string.IsNullOrEmpty(Item.NativeName))
            {
                DisplayName = char.ToUpperInvariant(Item.NativeName[0]) + (Item.NativeName.Length > 1 ? Item.NativeName.Substring(1) : string.Empty);
            }

            _selectionHandler = selectionHandler;
        }

        public void SetChecked(bool isChecked)
        {
            this.RaiseAndSetIfChanged(ref _isChecked, isChecked, nameof(IsChecked));
        }

        public void OnSelected()
        {
            _selectionHandler.Invoke(this);
        }
    }
}
