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

using System.Reactive.Disposables;
using Covi.Features.Analytics;
using Covi.Features.Regions;
using Covi.Utils;
using Prism.Navigation;

namespace Covi.Features.Menu
{
    public class MenuViewModel : CompositeViewModelBase
    {
        public const string MenuRegionIdentifier = nameof(MenuRegionIdentifier);

        public MenuViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            MenuRegion = RegionManager.CreateForIdentifier(MenuRegionIdentifier, HostContext);
        }

        public RegionManager MenuRegion { get; }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(MenuViewModel));
        }
    }
}
