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

using System.ComponentModel;
using Covi.Features.Controls.HtmlLabel;
using Covi.iOS.CustomRenderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelIosRenderer))]
namespace Covi.iOS.CustomRenderers
{
    //Source: https://github.com/edsnider/htmllabel ;
    //https://github.com/matteobortolazzo/HtmlLabelPlugin
    public class HtmlLabelIosRenderer : ViewRenderer<HtmlLabel, UITextView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<HtmlLabel> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    SetNativeControl(new UITextView());
                }

                UpdateText();
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(HtmlLabel.Text))
            {
                UpdateText();
            }
        }

        void UpdateText()
        {
            if (string.IsNullOrWhiteSpace(Element?.Text))
            {
                Control.Text = string.Empty;
                return;
            }

            var isRtl = Device.FlowDirection == FlowDirection.RightToLeft;
            var styledHtml = new RendererHelper(Element, Element.Text, Device.RuntimePlatform, isRtl).ToString();
            if (styledHtml != null)
            {
                SetText(styledHtml);
                SetNeedsDisplay();
            }
        }

        private void SetText(string styledHtml)
        {
            NSError error = null;
            Control.AttributedText = new NSAttributedString(NSData.FromString(styledHtml),
                new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML, StringEncoding = NSStringEncoding.UTF8 },
                ref error);
            Control.Editable = false;
            Control.ScrollEnabled = false;
            Control.ShouldInteractWithUrl += delegate { return true; };
        }
    }
}
