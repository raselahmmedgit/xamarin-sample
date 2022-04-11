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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Covi.Services.Navigation.Hints;
using DynamicData;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Covi.Features.Main
{
    public partial class MainPage : Xamarin.Forms.TabbedPage, INavigationPageOptions, IClearBackStack
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            On<Xamarin.Forms.PlatformConfiguration.Android>()
                .SetToolbarPlacement(ToolbarPlacement.Bottom)
                .SetIsSwipePagingEnabled(false);

            Children.AddRange(MainPageService.GetPages());
            UpdateCurrentPage();
        }

        private int PreviousPageIndex { get; set; }

        private bool ShouldNavigateToPreviousPage { get; set; }

        public float TabBarHeight { get; private set; }

        public void UpdateCurrentPage()
        {
            CurrentPage = Children[MainPageService.GetDefaultPageIndex()];
        }

        public void UpdateCurrentPage(Type pageType)
        {
            var page = Children.FirstOrDefault(c => c.GetType() == pageType);
            if (page != null)
            {
                CurrentPage = page;
            }
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);

            if (string.Equals(propertyName, nameof(CurrentPage)))
            {
                PreviousPageIndex = Children.IndexOf(CurrentPage);
            }
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage is ITabbedPageItemCustomHandler customHandleablePage)
            {
                customHandleablePage.HandleTabbedPageItemNavigation();
                SetPreviousPageAsActive();
            }
        }

        public void SetTabBarHeight(float height)
        {
            TabBarHeight = height;
        }

        public bool ClearNavigationStackOnNavigation => true;

        private void SetPreviousPageAsActive()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                // To retain activation/deactivation cycle of pages on Android it is required to revert navigation afterwards.
                ShouldNavigateToPreviousPage = true;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                CurrentPage = Children[PreviousPageIndex];
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ShouldNavigateToPreviousPage)
            {
                CurrentPage = Children[PreviousPageIndex];
                ShouldNavigateToPreviousPage = false;
            }
        }
    }
}
