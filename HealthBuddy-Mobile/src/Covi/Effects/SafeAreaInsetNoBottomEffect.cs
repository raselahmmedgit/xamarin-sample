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
    public class SafeAreaInsetNoBottomEffect
    {
        private const string UseSafeAreaInsetPropertyName = "UseSafeAreaNoBottomInsets";
        public const string EffectName = "SafeAreaPaddingNoBottomEffect";

        public static readonly BindableProperty UseSafeAreaInsetsProperty =
          BindableProperty.CreateAttached(UseSafeAreaInsetPropertyName,
              typeof(bool),
              typeof(SafeAreaInsetNoBottomEffect),
              false,
              propertyChanged: OnUseSafeAreaInsetsChanged);

        public static bool GetUseSafeAreaInsets(BindableObject view)
        {
            return (bool)view.GetValue(UseSafeAreaInsetsProperty);
        }

        public static void SetUseSafeAreaInsets(BindableObject view, bool value)
        {
            view.SetValue(UseSafeAreaInsetsProperty, value);
        }

        private static void OnUseSafeAreaInsetsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Layout;
            if (view == null)
            {
                return;
            }

            bool hasEffect = (bool)newValue;
            if (hasEffect)
            {
                view.Effects.Add(new SafeAreaPaddingNoBottomEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is SafeAreaPaddingNoBottomEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }

        internal class SafeAreaPaddingNoBottomEffect : RoutingEffect
        {
            public SafeAreaPaddingNoBottomEffect()
                : base($"{Constants.EffectsGroupName}.{EffectName}")
            {
            }
        }


    }
}
