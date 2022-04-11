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
using Covi.Features.Regions;
using Covi.Utils;
using Xamarin.Forms;

namespace Covi.Features.ComponentsManagement
{
    // TODO: migrate to CollectionView when control becomes stable
    // Steps to migrate:
    // 1. Update this class properties (e.g. use IsGrouped instead of IsGroupingEnabled)
    // 2. Find and remove all of the usages of the extension method ViewExtensions.WrapWithViewCell
    // 3. Update group header template with the method CompressedLayout.SetIsHeadless
    // 4. Remove any usages of the method ViewCell.ForceUpdateSize in the components
    // 5. Set ItemsLayout property accorging requirements
    public class ComponentsHost : ListView
    {
        private const string RegionManagerPropertyName = "RegionManager";
        private const string ComponentDataTemplateSelectorPropertyName = "ComponentDataTemplateSelector";

        public static readonly BindableProperty RegionManagerProperty =
            BindableProperty.Create(RegionManagerPropertyName,
                typeof(RegionManager),
                typeof(ComponentsHost),
                null,
                propertyChanged: HandleRegionManagerPropertyChanged);

        public static readonly BindableProperty ComponentDataTemplateSelectorProperty =
            BindableProperty.Create(ComponentDataTemplateSelectorPropertyName,
                typeof(ComponentDataTemplateSelector),
                typeof(ComponentsHost),
                new ComponentDataTemplateSelector(),
                propertyChanged: HandleComponentDataTemplateSelectorPropertyChanged);

        private static void HandleRegionManagerPropertyChanged(Xamarin.Forms.BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as ComponentsHost;

            if (view == null)
            {
                return;
            }

            var regionManager = newValue as RegionManager;

            view.SetRegionManager(regionManager);
        }

        private static void HandleComponentDataTemplateSelectorPropertyChanged(Xamarin.Forms.BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as ComponentsHost;

            if (view == null)
            {
                return;
            }

            var dataTemplateSelector = newValue as ComponentDataTemplateSelector;
            view.SetComponentDataTemplateSelector(dataTemplateSelector);
        }

        public ComponentsHost()
        {
            SelectionMode = ListViewSelectionMode.None;
            HasUnevenRows = true;
            IsGroupingEnabled = true;
            BackgroundColor = Color.Transparent;
            SeparatorVisibility = SeparatorVisibility.None;
            GroupHeaderTemplate = GetDefaultGroupHeaderTemplate();
            ItemTemplate = ComponentDataTemplateSelector;
        }

        public ComponentDataTemplateSelector ComponentDataTemplateSelector
        {
            get => (ComponentDataTemplateSelector)GetValue(ComponentDataTemplateSelectorProperty);
            set => SetValue(ComponentDataTemplateSelectorProperty, value);
        }

        public RegionManager RegionManager
        {
            get => (RegionManager)GetValue(RegionManagerProperty);
            set => SetValue(RegionManagerProperty, value);
        }

        private void SetRegionManager(RegionManager regionManager)
        {
            ItemsSource = regionManager?.RegionItems;
        }

        private void SetComponentDataTemplateSelector(ComponentDataTemplateSelector dataTemplateSelector)
        {
            ItemTemplate = dataTemplateSelector;
        }

        private DataTemplate GetDefaultGroupHeaderTemplate()
        {
            return new DataTemplate(() =>
            {
                var result = new Grid();
                var cell = result.WrapWithViewCell();
                cell.Height = 0.01;

                // We should set margin and actual background color because of bug with lines on iPhone7
                // https://forums.xamarin.com/discussion/116820/listview-groupheader-line-that-im-unable-to-remove
                result.BackgroundColor = BackgroundColor;
                result.Margin = new Thickness(0, -0.3);
                return cell;
            });
        }
    }
}
