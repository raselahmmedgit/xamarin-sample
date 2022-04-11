using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Covi.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(ExtendedPickerRenderer))]

namespace Covi.Droid.CustomRenderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        private Context _context;
        public ExtendedPickerRenderer(Context context) : base(context)
        {
            this._context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control == null || e.NewElement == null) return;
            //for example ,change the line to red:
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.White);
            else
                Control.Background.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcAtop);
        }
    }
}
