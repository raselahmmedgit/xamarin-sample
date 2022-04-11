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

using Xamarin.Forms;

namespace Covi.Features.Controls
{
    public partial class BusyIndicatorView : ContentView
    {
        private const string IsActivePropertyName = "IsActive";
        private const string FadeBackgroundPropertyName = "FadeBackground";
        private const string IndicatorColorPropertyName = "IndicatorColor";


        public static readonly BindableProperty IsActiveProperty =
            BindableProperty.Create(IsActivePropertyName,
                typeof(bool),
                typeof(BusyIndicatorView),
                false,
                propertyChanged: HandleIsActivePropertyChanged);

        public static readonly BindableProperty FadeBackgroundProperty =
            BindableProperty.Create(FadeBackgroundPropertyName,
                typeof(bool),
                typeof(BusyIndicatorView),
                false,
                propertyChanged: HandleHasTransparentBackgroundPropertyChanged);

        public static readonly BindableProperty IndicatorColorProperty =
            BindableProperty.Create(IndicatorColorPropertyName,
                                    typeof(Color),
                                    typeof(BusyIndicatorView),
                                    Color.Default,
                                    propertyChanged: HandleIndicatorColorPropertyChanged);

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public bool FadeBackground
        {
            get => (bool)GetValue(FadeBackgroundProperty);
            set => SetValue(FadeBackgroundProperty, value);
        }

        public Color IndicatorColor
        {
            get => (Color)GetValue(IndicatorColorProperty);
            set => SetValue(IndicatorColorProperty, value);
        }

        private static void HandleHasTransparentBackgroundPropertyChanged(Xamarin.Forms.BindableObject bindable, object oldvalue, object newvalue)
        {
            var view = bindable as BusyIndicatorView;

            if (view == null)
            {
                return;
            }

            var hasFadedBackground = newvalue != null && (bool)newvalue;
            view.SetFadedBackground(hasFadedBackground);
        }

        private static void HandleIsActivePropertyChanged(Xamarin.Forms.BindableObject bindable, object oldvalue, object newvalue)
        {
            var view = bindable as BusyIndicatorView;

            if (view == null)
            {
                return;
            }

            bool isBusy = (bool)newvalue;
            view.SetActiveState(isBusy);
        }

        private static void HandleIndicatorColorPropertyChanged(Xamarin.Forms.BindableObject bindable, object oldvalue, object newvalue)
        {
            var view = bindable as BusyIndicatorView;

            if (view == null)
            {
                return;
            }

            if (oldvalue == newvalue || !(newvalue is Color newColor) || newColor == Color.Default)
            {
                return;
            }

            view.SetIndicatorColor((Color) newvalue);
        }

        private void SetActiveState(bool isActive)
        {
            IsVisible = isActive;

            BusyIndicator.IsRunning = isActive;
            BusyIndicator.IsVisible = isActive;
        }

        private void SetFadedBackground(bool hasFadedBackground)
        {
            BackgroundColor = hasFadedBackground ? Color.FromRgba(255, 255, 255, 60) : Color.Transparent;
        }

        private void SetIndicatorColor(Color color)
        {
            BusyIndicator.Color = color;
        }

        public BusyIndicatorView()
        {
            InitializeComponent();
        }
    }
}
