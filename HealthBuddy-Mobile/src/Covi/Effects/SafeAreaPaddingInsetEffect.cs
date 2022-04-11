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
    public class SafeAreaPaddingInsetEffect
    {
        private const string UseSafeAreaInsetPropertyName = "UseSafeAreaPaddingInsets";
        public const string EffectName = "SafeAreaPaddingEffect";

        public static readonly BindableProperty UseSafeAreaPaddingInsetsProperty =
          BindableProperty.CreateAttached(UseSafeAreaInsetPropertyName, typeof(InsetDirection), typeof(SafeAreaPaddingInsetEffect), InsetDirection.None, propertyChanged: OnUseSafeAreaInsetsChanged);

        public static InsetDirection GetUseSafeAreaPaddingInsets(BindableObject view)
        {
            return (InsetDirection)view.GetValue(UseSafeAreaPaddingInsetsProperty);
        }

        public static void SetUseSafeAreaPaddingInsets(BindableObject view, InsetDirection value)
        {
            view.SetValue(UseSafeAreaPaddingInsetsProperty, value);
        }

        private static void OnUseSafeAreaInsetsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Layout;
            if (view == null)
            {
                return;
            }

            if (!(newValue is InsetDirection value))
            {
                return;
            }
            var toRemoveOld = view.Effects.FirstOrDefault(e => e is SafeAreaPaddingEffect);
            if (toRemoveOld != null)
            {
                view.Effects.Remove(toRemoveOld);
            }

            if (value != InsetDirection.None)
            {
                view.Effects.Add(new SafeAreaPaddingEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is SafeAreaPaddingEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }

        internal class SafeAreaPaddingEffect : RoutingEffect
        {
            public SafeAreaPaddingEffect()
                : base($"{Constants.EffectsGroupName}.{EffectName}")
            {
            }
        }
    }
}
