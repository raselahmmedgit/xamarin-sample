using System;
using System.Drawing.Text;

using Covi.Effects;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportEffect(typeof(Covi.iOS.Effects.SafeAreaMarginEffect), Covi.Effects.SafeAreaMarginInsetEffect.EffectName)]
namespace Covi.iOS.Effects
{
    public class SafeAreaMarginEffect : PlatformEffect
    {
        Thickness _margin;
        Thickness? _originalMargin;
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
                    _margin = _originalMargin ?? element.Margin;
                    var insets = GetSafeAreaInsets();

                    if (_insets == insets)
                    {
                        return;
                    }

                    var leftMargin =
                        (SafeAreaMarginInsetEffect.GetUseSafeAreaMarginInsets(Element).HasFlag(InsetDirection.Left))
                            ? _margin.Left + insets.Left
                            : _margin.Left;

                    var topMargin = (SafeAreaMarginInsetEffect.GetUseSafeAreaMarginInsets(Element).HasFlag(InsetDirection.Top))
                                        ? _margin.Top + insets.Top
                                        : _margin.Top;

                    var rightMargin =
                        (SafeAreaMarginInsetEffect.GetUseSafeAreaMarginInsets(Element).HasFlag(InsetDirection.Right))
                            ? _margin.Right + insets.Right
                            : _margin.Right;

                    var bottomMargin =
                        (SafeAreaMarginInsetEffect.GetUseSafeAreaMarginInsets(Element).HasFlag(InsetDirection.Bottom))
                            ? _margin.Bottom + insets.Bottom
                            : _margin.Bottom;

                    element.Margin = new Thickness(leftMargin, topMargin,
                                                   rightMargin,
                                                   bottomMargin);

                    if (!_originalMargin.HasValue)
                    {
                        _originalMargin = _margin;
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
            if (_originalMargin.HasValue && Element is Layout element)
            {
                element.SizeChanged -= OnSizeChanged;
                element.Padding = _originalMargin.Value;
                _originalMargin = null;
            }
        }
    }
}
