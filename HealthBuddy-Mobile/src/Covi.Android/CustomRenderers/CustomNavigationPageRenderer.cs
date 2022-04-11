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
using Android.App;
using Android.Content;
using Covi.Droid.CustomRenderers;
using Covi.Features.Shell;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

using AppCompToolbar = Android.Support.V7.Widget.Toolbar;

[assembly: ExportRenderer(typeof(ShellPage), typeof(CustomNavigationPageRenderer))]
namespace Covi.Droid.CustomRenderers
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        IPageController PageController => Element;
        ShellPage CustomNavigationPage => Element as ShellPage;

        public CustomNavigationPageRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            CustomNavigationPage.IgnoreLayoutChange = true;
            base.OnLayout(changed, l, t, r, b);
            CustomNavigationPage.IgnoreLayoutChange = false;

            int containerHeight = b - t;

            PageController.ContainerArea = new Rectangle(0, 0, Context.FromPixels(r - l), Context.FromPixels(containerHeight));

            for (var i = 0; i < ChildCount; i++)
            {
                Android.Views.View child = GetChildAt(i);

                if (child is AppCompToolbar)
                {
                    continue;
                }

                child.Layout(0, 0, r, b);
            }

            var context = (Activity)Context;
            var toolbar = context.FindViewById<AppCompToolbar>(Droid.Resource.Id.toolbar);

            if (toolbar != null)
            {
                if (toolbar.NavigationIcon != null)
                {
                    toolbar.NavigationIcon = Android.Support.V7.Content.Res.AppCompatResources.GetDrawable(context, Resource.Drawable.back);
                }
            }
        }
    }
}
