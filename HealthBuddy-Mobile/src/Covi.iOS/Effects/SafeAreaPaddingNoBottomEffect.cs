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
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Covi.iOS.Effects
{
    public class SafeAreaPaddingNoBottomEffect : PlatformEffect
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

                    var insets = new UIEdgeInsets(20, 0, 0, 0);

                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        insets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
                    }

                    if (_insets == insets)
                    {
                        return;
                    }
                    if (insets.Top > 0 || insets.Bottom > 0 || insets.Left > 0 || insets.Right > 0)
                    {
                        element.Padding = new Thickness(_padding.Left + insets.Left,
                                                        _padding.Top + insets.Top,
                                                        _padding.Right + insets.Right,
                                                        _padding.Bottom);

                        if (!_originalPadding.HasValue)
                        {
                            _originalPadding = _padding;
                        }

                        _insets = insets;
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
                element.Padding = _originalPadding.Value;
                _originalPadding = null;
            }
        }
    }
}
