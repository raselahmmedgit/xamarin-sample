using Covi.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(ExtendedButtonRenderer))]
namespace Covi.iOS.CustomRenderers
{
    public class ExtendedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
            }
        }
    }
}
