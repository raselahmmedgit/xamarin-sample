﻿// =========================================================================
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

using Xamarin.Forms;

namespace Covi.PlatformSpecific
{
    public class DashedLine : View
    {
        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create("Color",
                typeof(Color),
                typeof(DashedLine),
                Color.Transparent);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty AlphaProperty =
            BindableProperty.Create(nameof(Alpha),
                typeof(double),
                typeof(DashedLine),
                255.0);

        public double Alpha
        {
            get => (double)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        public static readonly BindableProperty LineWidthProperty =
            BindableProperty.Create("LineWidth",
                typeof(int),
                typeof(DashedLine),
                5);

        public int LineWidth
        {
            get => (int)GetValue(LineWidthProperty);
            set => SetValue(LineWidthProperty, value);
        }

        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(int),
                typeof(DashedLine),
                5);

        public int Spacing
        {
            get => (int)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }
    }
}
