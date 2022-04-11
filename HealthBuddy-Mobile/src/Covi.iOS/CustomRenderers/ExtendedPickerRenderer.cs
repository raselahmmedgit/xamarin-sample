using System;
using Covi.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(ExtendedPickerRenderer))]
namespace Covi.iOS.CustomRenderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
                return;
            Control.Layer.BorderWidth = 1;
            Control.Layer.BorderColor = Color.Red.ToCGColor();
        }
    }
}
