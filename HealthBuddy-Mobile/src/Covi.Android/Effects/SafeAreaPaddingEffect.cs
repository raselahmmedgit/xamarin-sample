using System;
using Android.App;
using Android.Views;
using Covi.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(Covi.Droid.Effects.SafeAreaPaddingEffect), Covi.Effects.SafeAreaPaddingInsetEffect.EffectName)]
namespace Covi.Droid.Effects
{
    public class SafeAreaPaddingEffect : PlatformEffect
    {
        Thickness _padding;
        Thickness? _originalPadding;
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
                    _padding = _originalPadding ?? element.Padding;

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
                        var topPadding = SafeAreaPaddingInsetEffect.GetUseSafeAreaPaddingInsets(Element).HasFlag(InsetDirection.Top) ?
                                            _padding.Top + statusBarHeight : _padding.Top;
                        var newPadding = new Thickness(_padding.Left, topPadding, _padding.Right, _padding.Bottom);

                        element.Padding = newPadding;

                        if (!_originalPadding.HasValue)
                        {
                            _originalPadding = _padding;
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
            if (_originalPadding.HasValue && Element is Layout element)
            {
                element.SizeChanged -= OnSizeChanged;
                //element.Padding = _originalPadding.Value;
                _originalPadding = null;
            }
        }
    }
}
