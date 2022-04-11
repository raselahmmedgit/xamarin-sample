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
using Xamarin.Forms;

namespace Covi.Utils
{
    public static class ViewExtensions
    {
        public static ViewCell WrapWithViewCell(this View view, object bindingContext = null)
        {
            var result = new ViewCell();
            result.View = view;

            if (bindingContext != null)
            {
                view.BindingContext = bindingContext;
            }

            return result;
        }

        public static View WithMargin(this View view, Thickness margin)
        {
            view.Margin = margin;
            return view;
        }
    }
}
