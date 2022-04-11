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
using System.ComponentModel;

using Covi.Droid.Effects;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(NavBarBackgroundColorEffect), Covi.Effects.NavigationBarExtensions.EffectName)]
namespace Covi.Droid.Effects
{
    class NavBarBackgroundColorEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Element is Page page)
            {
                page.Appearing += OnAppearing;
            }
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            UpdateColor();
        }

        protected override void OnDetached()
        {
            if (Element is Page page)
            {
                page.Appearing -= OnAppearing;
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == Covi.Effects.NavigationBarExtensions.NavBarBackgroundColorPropertyName)
            {
                UpdateColor();
            }

        }

        private void UpdateColor()
        {
            try
            {
                var color = Covi.Effects.NavigationBarExtensions.GetBackgroundColor(Element);
                SetNavBarColor(color);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to set navigation bar background color. Error: ", ex.Message);
            }
        }

        private void SetNavBarColor(Color color)
        {
            var page = Element as Page;

            var navPage = page?.Parent as NavigationPage;
            if (navPage == null)
            {
                return;
            }

            navPage.BarBackgroundColor = color;
        }
    }
}
