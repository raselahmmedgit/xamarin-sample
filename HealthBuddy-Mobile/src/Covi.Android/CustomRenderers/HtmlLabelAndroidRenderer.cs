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
using Android.Content;
using Android.OS;
using Android.Text;
using Covi.Droid.CustomRenderers;
using Covi.Features.Controls.HtmlLabel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelAndroidRenderer))]

namespace Covi.Droid.CustomRenderers
{
    //Source:https://github.com/edsnider/htmllabel
    public class HtmlLabelAndroidRenderer : LabelRenderer
    {
        public HtmlLabelAndroidRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                UpdateText();
            }
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

            var htmlWithoutImageTag = Element.Text.RemoveImageTag();

            Control.TextFormatted = Build.VERSION.SdkInt >= BuildVersionCodes.N
                ? Html.FromHtml(htmlWithoutImageTag, FromHtmlOptions.ModeCompact)
#pragma warning disable CS0618 // Type or member is obsolete
                : Html.FromHtml(htmlWithoutImageTag);
#pragma warning restore CS0618 // Type or member is obsolete

            Control.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;
        }
    }
}
