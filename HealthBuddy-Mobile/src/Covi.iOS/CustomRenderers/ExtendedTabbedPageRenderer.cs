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
//https://xamgirl.com/extending-tabbedpage-in-xamarin-forms/

using System;
using CoreGraphics;
using Covi.Features.Main;
using Covi.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MainPage), typeof(ExtendedTabbedPageRenderer))]
namespace Covi.iOS.CustomRenderers
{
    public class ExtendedTabbedPageRenderer : TabbedRenderer
    {
        private nfloat _indicatorHeight = 3;
        private nfloat _previousTabBarHeight;

        public override void ViewWillAppear(bool animated)
        {
            AddSelectedTabIndicator();
            UpdateTabBarHeight();
            base.ViewWillAppear(animated);
        }

        private void UpdateTabBarHeight()
        {
            if (Element is MainPage mainPage)
            {
                mainPage.SetTabBarHeight((float)TabBar.Bounds.Size.Height);
            }
        }

        void AddSelectedTabIndicator()
        {
            if (ViewControllers != null)
            {
                nfloat? additionalHeight = 0;

                var currentTabBarHeight = TabBar.Bounds.Size.Height;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    if (_previousTabBarHeight == 0 || currentTabBarHeight < _previousTabBarHeight)
                    {
                        additionalHeight = UIApplication.SharedApplication.KeyWindow?.SafeAreaInsets.Bottom;
                    }
                }

                var heightValue = additionalHeight.HasValue ? additionalHeight.Value : 0;
                _previousTabBarHeight = currentTabBarHeight;

                UITabBar.Appearance.SelectionIndicatorImage = GetImageWithColorPosition(
                    new CGSize(UIScreen.MainScreen.Bounds.Width / ViewControllers.Length, currentTabBarHeight + heightValue),
                    new CGSize((UIScreen.MainScreen.Bounds.Width / ViewControllers.Length)/2, _indicatorHeight));
            }
        }

        UIImage GetImageWithColorPosition(CGSize size, CGSize lineSize)
        {
            var rect = new CGRect(0, 0, size.Width, size.Height);
            var rectLine = new CGRect(lineSize.Width / 2, 0, lineSize.Width, lineSize.Height);
            UIGraphics.BeginImageContextWithOptions(size, false, 0);
            UIColor.Clear.SetFill();
            UIGraphics.RectFill(rect);
            var color = (this.Element as TabbedPage).SelectedTabColor.ToUIColor();
            color.SetFill();
            UIGraphics.RectFill(rectLine);
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return img;

        }
    }
}
