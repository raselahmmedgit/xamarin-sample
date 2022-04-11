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

using System.ComponentModel;
using Covi.Features.ComponentsManagement;
using Covi.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ComponentsHost), typeof(ExtendedListViewRenderer))]
namespace Covi.iOS.CustomRenderers
{
    public class ExtendedListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (Element != null)
            {
                Control.AlwaysBounceVertical = Element.IsPullToRefreshEnabled;
                Control.Bounces = Element.IsPullToRefreshEnabled;
                UpdateSelectionMode();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Element.IsPullToRefreshEnabled))
            {
                Control.AlwaysBounceVertical = Element.IsPullToRefreshEnabled;
                Control.Bounces = Element.IsPullToRefreshEnabled;
            }

            if (e.PropertyName == nameof(ListView.SelectionMode))
            {
                UpdateSelectionMode();
            }
        }

        private void UpdateSelectionMode()
        {
            if (Element is ListView listView &&
                Control is UITableView tableView)
            {
                switch (listView.SelectionMode)
                {
                    case ListViewSelectionMode.None:
                        tableView.AllowsSelection = false;
                        break;
                    case ListViewSelectionMode.Single:
                        tableView.AllowsSelection = true;
                        break;
                }
            }
        }
    }
}
