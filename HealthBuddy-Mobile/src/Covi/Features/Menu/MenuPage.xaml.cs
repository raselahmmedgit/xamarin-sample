﻿// =========================================================================
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

using Covi.PlatformSpecific;
using Xamarin.Forms;

namespace Covi.Features.Menu
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IStatusBarStyle>().ChangeTextColor(StatusBarTheme.Light);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IStatusBarStyle>().ChangeTextColor(StatusBarTheme.Dark);
            }
        }
    }
}
