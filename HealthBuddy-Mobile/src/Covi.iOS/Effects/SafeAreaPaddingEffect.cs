#pragma warning disable SA1636 // File header copyright text should match
//https://xamarinhelp.com/safeareainsets-xamarin-forms-ios/
#pragma warning restore SA1636 // File header copyright text should match

using System;

using Covi.Effects;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(Covi.iOS.Effects.SafeAreaPaddingEffect), Covi.Effects.SafeAreaPaddingInsetEffect.EffectName)]
namespace Covi.iOS.Effects
{
    public class SafeAreaPaddingEffect : PlatformEffect
    {
        Thickness _padding;
        Thickness? _originalPadding;
        UIEdgeInsets _insets;

        protected override void OnAttached()
        {
            try
            {
                if (Element is Layout element)
                {
                    if (Control is UIScrollView scroll)
                    {
                        scroll.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
                    }
                    TryUpdateOffsets();
                    element.SizeChanged += OnSizeChanged;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            TryUpdateOffsets();
        }

        private void TryUpdateOffsets()
        {
            try
            {
                if (Element is Layout element)
                {
                    _padding = _originalPadding ?? element.Padding;
                    var insets = GetSafeAreaInsets();

                    if (_insets == insets)
                    {
                        return;
                    }

                    var left = (SafeAreaPaddingInsetEffect.GetUseSafeAreaPaddingInsets(Element).HasFlag(InsetDirection.Left))
                                   ? _padding.Left + insets.Left
                                   : _padding.Left;

                    var top = (SafeAreaPaddingInsetEffect.GetUseSafeAreaPaddingInsets(Element).HasFlag(InsetDirection.Top))
                                  ? _padding.Top + insets.Top
                                  : _padding.Top;

                    var right = (SafeAreaPaddingInsetEffect.GetUseSafeAreaPaddingInsets(Element).HasFlag(InsetDirection.Right))
                                    ? _padding.Right + insets.Right
                                    : _padding.Right;

                    var bottom =
                        (SafeAreaPaddingInsetEffect.GetUseSafeAreaPaddingInsets(Element).HasFlag(InsetDirection.Bottom))
                            ? _padding.Bottom + insets.Bottom
                            : _padding.Bottom;

                    element.Padding = new Thickness(left, top,
                                                    right,
                                                    bottom);

                    if (!_originalPadding.HasValue)
                    {
                        _originalPadding = _padding;
                    }

                    _insets = insets;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        private UIEdgeInsets GetSafeAreaInsets()
        {
            UIEdgeInsets safeAreaInsets;

            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                safeAreaInsets = new UIEdgeInsets(UIApplication.SharedApplication.StatusBarFrame.Size.Height, 0, 0, 0);
            if (GetKeyWindow() != null)
                safeAreaInsets = GetKeyWindow().SafeAreaInsets;
            else if (UIApplication.SharedApplication.Windows.Length > 0)
                safeAreaInsets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
            else
                safeAreaInsets = UIEdgeInsets.Zero;

            return safeAreaInsets;
        }

        public static UIWindow GetKeyWindow()
        {
            var windows = UIApplication.SharedApplication.Windows;

            for (int i = 0; i < windows.Length; i++)
            {
                var window = windows[i];
                if (window.IsKeyWindow)
                    return window;
            }

            return null;
        }

        protected override void OnDetached()
        {
            if (_originalPadding.HasValue && Element is Layout element)
            {
                element.SizeChanged -= OnSizeChanged;
                element.Padding = _originalPadding.Value;
                _originalPadding = null;
            }
        }
    }
}
