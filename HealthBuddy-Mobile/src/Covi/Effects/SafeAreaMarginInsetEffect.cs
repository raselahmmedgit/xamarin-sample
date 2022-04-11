using System;
using System.Linq;
using Xamarin.Forms;

namespace Covi.Effects
{
    public class SafeAreaMarginInsetEffect
    {
        private const string UseSafeAreaInsetPropertyName = "UseSafeAreaMarginInsets";
        public const string EffectName = "SafeAreaMarginEffect";

        public static readonly BindableProperty UseSafeAreaMarginInsetsProperty =
            BindableProperty.CreateAttached(UseSafeAreaInsetPropertyName, typeof(InsetDirection), typeof(SafeAreaMarginInsetEffect), InsetDirection.None, propertyChanged: OnUseSafeAreaInsetsChanged);

        public static InsetDirection GetUseSafeAreaMarginInsets(BindableObject view)
        {
            return (InsetDirection)view.GetValue(UseSafeAreaMarginInsetsProperty);
        }

        public static void SetUseSafeAreaMarginInsets(BindableObject view, InsetDirection value)
        {
            view.SetValue(UseSafeAreaMarginInsetsProperty, value);
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
            var toRemoveOld = view.Effects.FirstOrDefault(e => e is SafeAreaMarginEffect);
            if (toRemoveOld != null)
            {
                view.Effects.Remove(toRemoveOld);
            }

            if (value != InsetDirection.None)
            {
                view.Effects.Add(new SafeAreaMarginEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is SafeAreaMarginEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }

        internal class SafeAreaMarginEffect : RoutingEffect
        {
            public SafeAreaMarginEffect()
                : base($"{Constants.EffectsGroupName}.{EffectName}")
            {
            }
        }
    }

    [Flags]
    public enum InsetDirection
    {
        None = 0,
        Right = 1,
        Left = 2,
        Top = 4,
        Bottom = 8,
        All = Left | Top | Right | Bottom
    }
}
