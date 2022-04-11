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
using Xamarin.Forms;

namespace Covi.Features.ComponentsManagement
{
    public class InfiniteScrollBehavior : Behavior<ComponentsHost>
    {
        protected override void OnAttachedTo(ComponentsHost bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.ItemAppearing += OnListViewItemAppearing;
        }

        protected override void OnDetachingFrom(ComponentsHost bindable)
        {
            bindable.ItemAppearing -= OnListViewItemAppearing;

            base.OnDetachingFrom(bindable);
        }

        private void OnListViewItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (sender is ComponentsHost componentsHost
                && componentsHost.RegionManager != null
                && e.Item is IComponent component)
            {
                foreach (var componentsGroup in componentsHost.RegionManager.RegionItems.ToList())
                {
                    if (!(componentsHost.RegionManager.GetComponentServiceByKey(componentsGroup.Key) is IInfiniteLoadable infiniteLoadable))
                    {
                        // If component service doesn't support infinite scrolling, ignore it.
                        continue;
                    }

                    int index = 0;
                    var group = componentsGroup.ToList();
                    foreach (var item in group)
                    {
                        if (item == component)
                        {
                            // 'index' variable is the index of the about-to-display component,
                            // if it is less than the amount of components in this component service by 1 then try to load additional items.
                            if (group.Count <= index + 2)
                            {
                                infiniteLoadable.LoadMoreIfNeeded();
                            }

                            return;
                        }

                        index++;
                    }
                }
            }
        }
    }
}
