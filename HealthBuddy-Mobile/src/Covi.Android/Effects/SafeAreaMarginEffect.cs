using System;

using Covi.Effects;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(Covi.Droid.Effects.SafeAreaMarginEffect), Covi.Effects.SafeAreaMarginInsetEffect.EffectName)]
namespace Covi.Droid.Effects
{
    public class SafeAreaMarginEffect : PlatformEffect
    {
        Thickness _margin;
        Thickness? _originalMargin;
        private float _statusBarHeight = 0f;

        protected override void OnAttached()
        {
            try
            {
                if (Element is Layout element)
                {
                    element.SizeChanged += OnSizeChanged;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected void OnSizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (Element is Layout element)
                {
                    _margin = _originalMargin ?? element.Margin;

                    float statusBarHeight = 0.0f;
                    int resourceId = Android.App.Application.Context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
                    if (resourceId > 0)
                    {
                        statusBarHeight = Android.App.Application.Context.Resources.GetDimensionPixelSize(resourceId) / Android.App.Application.Context.Resources.DisplayMetrics.Density;
                    }

                    if (Math.Abs(_statusBarHeight - statusBarHeight) < 0)
                    {
                        return;
                    }

                    if (statusBarHeight > 0)
                    {
                        var topMargin = SafeAreaMarginInsetEffect.GetUseSafeAreaMarginInsets(Element).HasFlag(InsetDirection.Top) ?
                                            _margin.Top + statusBarHeight : _margin.Top;
                        var newMargin = new Thickness(_margin.Left, topMargin, _margin.Right, _margin.Bottom);

                        element.Margin = newMargin;

                        if (!_originalMargin.HasValue)
                        {
                            _originalMargin = _margin;
                        }

                        _statusBarHeight = statusBarHeight;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            if (_originalMargin.HasValue && Element is Layout element)
            {
                element.SizeChanged -= OnSizeChanged;
                //element.Padding = _originalMargin.Value;
                _originalMargin = null;
            }
        }
    }
}
