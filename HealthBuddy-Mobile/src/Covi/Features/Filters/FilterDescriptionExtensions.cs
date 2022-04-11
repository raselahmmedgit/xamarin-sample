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
using System.Linq;

using Covi.Client.Services.Platform.Models;
using Covi.Features.Filters.Dialogs;

namespace Covi.Features.Filters
{
    public static class FilterDescriptionExtensions
    {
        public static FilterDescription ToFilterDescription(this Filter filter)
        {
            if (filter == null)
            {
                return null;
            }

            var filterValues = filter.Values
                .Select(v => new FilterOptionItem()
                {
                    DisplayName = v.DisplayName,
                    Value = v.ParameterName,
                    IsDefault = v.IsDefault ?? false,
                    IsChecked = v.IsDefault ?? false
                })
                .ToList();
            return new FilterDescription(filter.DisplayName, filter.ParameterName, filterValues, filter.IsMultiSelect ?? false);
        }
    }
}
