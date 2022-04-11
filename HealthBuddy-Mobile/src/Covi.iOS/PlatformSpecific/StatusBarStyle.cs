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

using Covi.iOS.PlatformSpecific;
using Covi.PlatformSpecific;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBarStyle))]
namespace Covi.iOS.PlatformSpecific
{
    public class StatusBarStyle : IStatusBarStyle
    {
        public void ChangeTextColor(StatusBarTheme theme)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var currentUIViewController = GetCurrentViewController();
                var style = theme == StatusBarTheme.Dark ? UIStatusBarStyle.DarkContent : UIStatusBarStyle.LightContent;
                UIApplication.SharedApplication.SetStatusBarStyle(style, false);
                currentUIViewController.SetNeedsStatusBarAppearanceUpdate();
            });
        }

        UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
    }
}
