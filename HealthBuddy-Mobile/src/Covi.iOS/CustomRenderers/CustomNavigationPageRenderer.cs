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

//https://xamgirl.com/transparent-navigation-bar-in-xamarin-forms/
//https://liudeyun.net/xamarin-custom-navbar-icon-text/
using System.Threading.Tasks;
using Covi.Features.Shell;
using Covi.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ShellPage), typeof(CustomNavigationPageRenderer))]
namespace Covi.iOS.CustomRenderers
{
    public class CustomNavigationPageRenderer : NavigationRenderer
    {
        protected override Task<bool> OnPushAsync(Page page, bool animated)
        {
            var retVal = base.OnPushAsync(page, animated);

            SetBackButtonImage("back");
            return retVal;
        }

        public void SetBackButtonImage(string imageBundleName)
        {
            var topViewController = this.TopViewController;

            // Create the image back button
            var backButtonImage = new UIBarButtonItem(
                UIImage.FromBundle(imageBundleName),
                UIBarButtonItemStyle.Plain,
                (sender, args) => { topViewController.NavigationController.PopViewController(true); });

            // Add buttons to the Top Bar
            UIBarButtonItem[] buttons = new UIBarButtonItem[1];
            buttons[0] = backButtonImage;

            topViewController.NavigationItem.LeftBarButtonItems = buttons;
            topViewController.NavigationController.NavigationBar.Translucent = true;
        }
    }
}
