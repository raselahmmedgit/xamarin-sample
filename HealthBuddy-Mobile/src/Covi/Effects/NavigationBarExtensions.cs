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

using System.Linq;
using Xamarin.Forms;

namespace Covi.Effects
{
    public static class NavigationBarExtensions
    {
        public const string NavBarBackgroundColorPropertyName = "NavBarBackgroundColor";
        public const string EffectName = "NavBarBackgroundColorEffect";

        public static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.CreateAttached(NavBarBackgroundColorPropertyName, typeof(Color?), typeof(Page), null, propertyChanged: OnNavBarBackgroundColorChanged);

        public static Color GetBackgroundColor(BindableObject view)
        {
            var color = view.GetValue(BackgroundColorProperty);
            return (Color?) color ?? Color.Transparent;
        }

        public static void SetBackgroundColor(BindableObject view, Color? color)
        {
            view.SetValue(BackgroundColorProperty, color);
        }

        private static void OnNavBarBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Page;
            if (view == null)
            {
                return;
            }

            var effect = view.Effects.FirstOrDefault(x => x is NavBarBackgroundColorEffectInternal) as NavBarBackgroundColorEffectInternal;

            if (effect == null)
            {
                view.Effects.Add(new NavBarBackgroundColorEffectInternal());
            }
        }

        internal class NavBarBackgroundColorEffectInternal : RoutingEffect
        {
            public NavBarBackgroundColorEffectInternal()
                : base($"{Constants.EffectsGroupName}.{EffectName}")
            {
            }
        }
    }
}
