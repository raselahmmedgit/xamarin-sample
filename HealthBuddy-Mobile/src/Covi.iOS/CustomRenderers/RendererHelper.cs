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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covi.Features.Controls.HtmlLabel;
using Xamarin.Forms;

namespace Covi.iOS.CustomRenderers
{
    // Source: https://github.com/matteobortolazzo/HtmlLabelPlugin
    public class RendererHelper
    {
        private readonly Label _label;
        private readonly string _runtimePlatform;
        private readonly bool _isRtl;
        private readonly string _text;
        private readonly IList<KeyValuePair<string, string>> _styles;

        public RendererHelper(Label label, string text, string runtimePlatform, bool isRtl)
        {
            _label = label ?? throw new ArgumentNullException(nameof(label));
            _runtimePlatform = runtimePlatform;
            _isRtl = isRtl;
            _text = text?.Trim();
            _text = _text?.RemoveImageTag();
            _styles = new List<KeyValuePair<string, string>>();
        }

        public void AddFontAttributesStyle(FontAttributes fontAttributes)
        {
            if (fontAttributes == FontAttributes.Bold)
            {
                AddStyle("font-weight", "bold");
            }
            else if (fontAttributes == FontAttributes.Italic)
            {
                AddStyle("font-style", "italic");
            }
        }

        public void AddFontFamilyStyle(string fontFamily)
        {
            string GetSystemFont() => _runtimePlatform switch
            {
                Device.iOS => "-apple-system",
                Device.Android => "Roboto",
                Device.UWP => "Segoe UI",
                _ => "system-ui",
            };

            var fontFamilyValue = string.IsNullOrWhiteSpace(fontFamily)
                ? GetSystemFont()
                : fontFamily;
            AddStyle("font-family", $"'{fontFamilyValue}'");
        }

        public void AddFontSizeStyle(double fontSize)
        {
            AddStyle("font-size", $"{fontSize}px");
        }

        public void AddTextColorStyle(Color color)
        {
            if (color.IsDefault)
            {
                return;
            }

            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = color.A;
            var hex = $"#{red:X2}{green:X2}{blue:X2}";
            var rgba = $"rgba({red},{green},{blue},{alpha})";
            AddStyle("color", hex);
            AddStyle("color", rgba);
        }

        public void AddHorizontalTextAlignStyle(TextAlignment textAlignment)
        {
            if (textAlignment == TextAlignment.Start)
            {
                AddStyle("text-align", _isRtl ? "right" : "left");
            }
            else if (textAlignment == TextAlignment.Center)
            {
                AddStyle("text-align", "center");
            }
            else if (textAlignment == TextAlignment.End)
            {
                AddStyle("text-align", _isRtl ? "left" : "right");
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(_text))
            {
                return null;
            }

            AddFontAttributesStyle(_label.FontAttributes);
            AddFontFamilyStyle(_label.FontFamily);
            AddTextColorStyle(_label.TextColor);
            AddHorizontalTextAlignStyle(_label.HorizontalTextAlignment);
            AddFontSizeStyle(_label.FontSize);

            var style = GetStyle();
            return $"<div style=\"{style}\" dir=\"auto\">{_text}</div>";
        }

        public string GetStyle()
        {
            var builder = new StringBuilder();

            foreach (KeyValuePair<string, string> style in _styles)
            {
                _ = builder.Append($"{style.Key}:{style.Value};");
            }

            var css = builder.ToString();
            if (_styles.Any())
            {
                css = css.Substring(0, css.Length - 1);
            }

            return css;
        }
        private void AddStyle(string selector, string value)
        {
            _styles.Add(new KeyValuePair<string, string>(selector, value));
        }
    }
}
