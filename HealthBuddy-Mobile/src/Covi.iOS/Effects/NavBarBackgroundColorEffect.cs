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
using System.ComponentModel;
using System.Linq;

using Covi.iOS.Effects;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(NavBarBackgroundColorEffect), Covi.Effects.NavigationBarExtensions.EffectName)]
namespace Covi.iOS.Effects
{
    class NavBarBackgroundColorEffect : PlatformEffect
    {
        private nint _statusBarTag = 11111;

        protected override void OnAttached()
        {
            if (Element is Page page)
            {
                page.Appearing += OnAppearing;
                page.Disappearing += OnDisappearing;
            }
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            UpdateColor();
        }

        // This piece of code handles status bar color for IPhone7 and IPhone7 Plus when we press back
        // button because main approach Xamarin.Forms.NavigationPage.BarBackgroundColor = color isn't
        // working for these specific devices.
        private void OnDisappearing(object sender, EventArgs e)
        {
            if (DeviceIsIPhone7())
            {
                SetStatusBarColorIPhone7(UIColor.Clear);
            }
        }

        protected override void OnDetached()
        {
            if (Element is Page page)
            {
                page.Appearing -= OnAppearing;
                page.Disappearing -= OnDisappearing;
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == Covi.Effects.NavigationBarExtensions.NavBarBackgroundColorPropertyName)
            {
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            try
            {
                var color = Covi.Effects.NavigationBarExtensions.GetBackgroundColor(Element);
                SetNavBarColor(color);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to set navigation bar background color. Error: ", ex.Message);
            }
        }

        private void SetNavBarColor(Color color)
        {
            var page = Element as Page;

            var navPage = page?.Parent as NavigationPage;
            if (navPage == null)
            {
                return;
            }

            navPage.BarBackgroundColor = color;

            var renderer = Platform.GetRenderer(page);
            var navigationController = renderer?.ViewController?.NavigationController;
            if (navigationController?.NavigationBar != null)
            {
                // This piece of code handles navigation bar color and status bar color for IPhone7 and IPhone7 Plus
                // because main approach Xamarin.Forms.NavigationPage.BarBackgroundColor = color isn't working for
                // these specific devices.
                if (DeviceIsIPhone7())
                {
                    var colorToSet = color.ToUIColor();
                    SetNavBarColorIPhone7(colorToSet, navigationController.NavigationBar);
                    SetStatusBarColorIPhone7(colorToSet);
                }

                navigationController.NavigationBar.ShadowImage = new UIImage();
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    navigationController.NavigationBar.StandardAppearance.ShadowColor = null;
                }

                navigationController.NavigationBar.Translucent = true;

                // In case of transparent color - need to remove shadow
                if (color == Color.Transparent)
                {
                    navigationController.NavigationBar.ShadowImage = new UIImage();
                    navigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        navigationController.NavigationBar.StandardAppearance.ShadowColor = null;
                    }


                }
            }
        }

        private void SetNavBarColorIPhone7(UIColor color, UINavigationBar navigationBar)
        {
            navigationBar.BackgroundColor = color;
        }

        private void SetStatusBarColorIPhone7(UIColor color)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                UIView statusBar = GetStatusBarCustomView();
                if (statusBar == null)
                {
                    statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                    statusBar.Tag = _statusBarTag;
                    UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                }
                else
                {
                    statusBar.BackgroundColor = color;
                }
            }
            else
            {
                UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                if (statusBar != null && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                {
                    statusBar.BackgroundColor = color;
                }
            }
        }

        private UIView GetStatusBarCustomView()
        {
            return UIApplication.SharedApplication.KeyWindow.Subviews.FirstOrDefault( x=> x.Tag == _statusBarTag);
        }

        private bool DeviceIsIPhone7()
        {
            var model = Xamarin.Essentials.DeviceInfo.Model;

            // ************
            // iPhone 7
            // ************
            // Model(s): A1660, A1778, A1779 & A1780
            // Apple Tech specs: https://support.apple.com/kb/SP743
            if (model == "iPhone9,1" || model == "iPhone9,3")
            {
                return true;
            }

            // ************
            // iPhone 7 Plus
            // ************
            // Model(s): A1661, A1784, A1785 and A1786
            // Apple Tech specs: https://support.apple.com/kb/SP744
            if (model == "iPhone9,2" || model == "iPhone9,4")
            {
                return true;
            }

            return false;
        }
    }
}
