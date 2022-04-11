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
using Covi.Effects;
using Covi.Utils;
using Prism.Behaviors;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covi.Behaviors
{
    class FadeNavBarOnScrollBehavior : BehaviorBase<Xamarin.Forms.ScrollView>
    {

        private double _initialPosition = 0;
        private Color? _originalColor = null;
        private bool _barModified = false;

        public static readonly BindableProperty TargetPageProperty = BindableProperty.Create(nameof(TargetPage), typeof(Xamarin.Forms.Page), typeof(FadeNavBarOnScrollBehavior), null);

        public static readonly BindableProperty FadeColorProperty =
            BindableProperty.Create(nameof(FadeColor), typeof(Color?), typeof(FadeNavBarOnScrollBehavior), Color.Transparent);

        public static readonly BindableProperty TresholdProperty = BindableProperty.Create(nameof(Treshold), typeof(double), typeof(FadeNavBarOnScrollBehavior), 1.0);


        public Color FadeColor
        {
            get { return (Color)GetValue(FadeColorProperty); }
            set { SetValue(FadeColorProperty, value); }
        }

        public Xamarin.Forms.Page TargetPage
        {
            get { return (Xamarin.Forms.Page)GetValue(TargetPageProperty); }
            set { SetValue(TargetPageProperty, value); }
        }

        public double Treshold
        {
            get { return (double)GetValue(TresholdProperty); }
            set { SetValue(TresholdProperty, value); }
        }

        protected override void OnAttachedTo(Xamarin.Forms.ScrollView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Scrolled += OnScrolled;
        }

        protected override void OnDetachingFrom(Xamarin.Forms.ScrollView bindable)
        {
            bindable.Scrolled -= OnScrolled;
            TargetPage = null;
            base.OnDetachingFrom(bindable);
            _initialPosition = 0;
        }

        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            // If no target page defined - try to find
            if (TargetPage == null)
            {
                TargetPage = this.AssociatedObject?.TryFindParent<Xamarin.Forms.Page>();
                _initialPosition = e.ScrollY;
            }

            var navPage = TargetPage?.Parent as Xamarin.Forms.NavigationPage;
            if (navPage == null)
            {
                return;
            }

            var delta = Math.Abs(e.ScrollY - _initialPosition);

            var scaledTreshold = Treshold * DeviceDisplay.MainDisplayInfo.Density;

            if (delta >= 10 && e.ScrollY > _initialPosition)
            {
                if (_originalColor == null && !_barModified)
                {
                    _originalColor = NavigationBarExtensions.GetBackgroundColor(TargetPage);
                }

                var targetColor = FadeColor;
                var alpha = Math.Min(delta / scaledTreshold, 1);
                var color = Color.FromRgba(targetColor.R, targetColor.G, targetColor.B, alpha);
                NavigationBarExtensions.SetBackgroundColor(TargetPage, color);
                _barModified = true;
            }
            else if (_barModified)
            {
                NavigationBarExtensions.SetBackgroundColor(TargetPage, _originalColor ?? Color.Default);
                _originalColor = null;
                _barModified = false;
            }
        }
    }
}
