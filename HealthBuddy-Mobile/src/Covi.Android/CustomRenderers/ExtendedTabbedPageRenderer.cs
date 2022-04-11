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

using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Covi.Droid.CustomRenderers;
using Covi.Features.Main;
using Google.Android.Material.BottomNavigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(MainPage), typeof(ExtendedTabbedPageRenderer))]
namespace Covi.Droid.CustomRenderers
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
        private int _indicatorHeight = 8;
        private bool _firstRun = true;
        private BottomNavigationView _bottomNavigationView;

        public ExtendedTabbedPageRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var tabsView = GetChildAt(0);

                if (tabsView != null)
                {
                    var tabsChildView = (tabsView as Android.Widget.RelativeLayout)?.GetChildAt(1);

                    if (tabsChildView != null)
                    {
                        _bottomNavigationView = (tabsChildView as BottomNavigationView);

                        if (_bottomNavigationView != null)
                        {
                            _bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;
                        }
                    }
                }
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (_firstRun && _bottomNavigationView != null)
            {
                UpdateTabBarHeight();
                for (int i = 0; i < Element.Children.Count; i++)
                {
                    var item = _bottomNavigationView.Menu?.GetItem(i);
                    if (item != null && _bottomNavigationView.SelectedItemId == item.ItemId)
                    {
                        SetupBottomNavigationView(item);
                        break;
                    }
                }
                _firstRun = false;
            }
        }

        private void UpdateTabBarHeight()
        {
            if (Element is MainPage mainPage)
            {
                mainPage.SetTabBarHeight(_bottomNavigationView.Height);
            }
        }

        void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            SetupBottomNavigationView(e.Item);
            this.OnNavigationItemSelected(e.Item);
        }

        //Adding line view
        void SetupBottomNavigationView(IMenuItem item)
        {
            int itemHeight = _bottomNavigationView.Height - (_indicatorHeight + 1);
            int itemWidth = (_bottomNavigationView.Width / Element.Children.Count);
            var innerOffset = itemWidth / 4;
            int leftOffset = (item.ItemId * itemWidth) + innerOffset;
            int rightOffset = (itemWidth * (Element.Children.Count - (item.ItemId + 1))) + innerOffset;
            GradientDrawable bottomLine = new GradientDrawable();
            bottomLine.SetShape(ShapeType.Line);
            bottomLine.SetStroke(_indicatorHeight, Element.SelectedTabColor.ToAndroid());
            var layerDrawable = new LayerDrawable(new Drawable[] { bottomLine });
            layerDrawable.SetLayerInset(0, leftOffset, 0, rightOffset, itemHeight);

            _bottomNavigationView.SetBackground(layerDrawable);
        }
    }
}
