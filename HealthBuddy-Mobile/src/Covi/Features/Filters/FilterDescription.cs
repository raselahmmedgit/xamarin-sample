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
using Covi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Covi.Features.Filters
{
    public class FilterDescription : ICloneable
    {
        public FilterDescription(
            string displayName,
            string parameterName,
            List<FilterOptionItem> values,
            bool multiSelect)
        {
            DisplayName = displayName;
            ParameterName = parameterName;
            Values = values;
            MultiSelect = multiSelect;
        }

        public string ParameterName { get; }
        public string DisplayName { get; }
        public List<FilterOptionItem> Values { get; }
        public bool MultiSelect { get; }

        public object Clone()
        {
            return new FilterDescription(DisplayName, ParameterName, Values.CloneValueList().ToList(), MultiSelect);
        }
    }
}
