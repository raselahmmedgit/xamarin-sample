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

using System.Linq;
using Covi.Services.Navigation.Hints;
using Prism.AppModel;
using Xamarin.Forms;

namespace Covi.Features.Shell
{
    public partial class ShellPage : NavigationPage, IPageLifecycleAware
    {
        public bool IgnoreLayoutChange { get; set; } = false;

        protected override void OnSizeAllocated(double width, double height)
        {
            if (!IgnoreLayoutChange)
            {
                base.OnSizeAllocated(width, height);
            }
        }

        public ShellPage()
        {
            InitializeComponent();
            Pushed += ShellPagePushed;
        }

        private void ShellPagePushed(object sender, NavigationEventArgs e)
        {
            var navpage = e.Page;
            if (navpage is IClearBackStack)
            {
                ClearBackStack();
            }
        }

        private void ClearBackStack()
        {
            if (StackDepth == 1)
            {
                return;
            }

            Element[] childrenToRemove = InternalChildren.Take(InternalChildren.Count()-1).ToArray();

            foreach (Element child in childrenToRemove)
            {
                InternalChildren.Remove(child);
            }
        }

        // This method allows us to avoid bug on iOs when page appearing event doesn't call after
        // restoring app from background. This bug is fixed in upcoming version of Xamarin.Forms 4.8.0
        // so we should remove this method and inheritance from IPageLifecycleAware after update
        // Xamarin.Forms to version 4.8.0 https://github.com/xamarin/Xamarin.Forms/pull/11172
        void IPageLifecycleAware.OnAppearing()
        {
            (CurrentPage?.BindingContext as IPageLifecycleAware)?.OnAppearing();
        }

        // This method allows us to avoid bug on iOs when page appearing event doesn't call after
        // restoring app from background. This bug is fixed in upcoming version of Xamarin.Forms 4.8.0
        // so we should remove this method and inheritance from IPageLifecycleAware after update
        // Xamarin.Forms to version 4.8.0 https://github.com/xamarin/Xamarin.Forms/pull/11172
        void IPageLifecycleAware.OnDisappearing()
        {
            (CurrentPage?.BindingContext as IPageLifecycleAware)?.OnDisappearing();
        }
    }
}
