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
// https://github.com/xamarin/Xamarin.Forms/issues/3463

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Covi.Droid.CustomRenderers;
using Covi.Features.ComponentsManagement;
using Android.Graphics.Drawables;
using System.ComponentModel;
using Android.Content;

[assembly: ExportRenderer(typeof(ComponentsHost), typeof(ExtendedListViewRenderer))]
namespace Covi.Droid.CustomRenderers
{
    public class ExtendedListViewRenderer : ListViewRenderer
    {
        private Drawable _selectorDrawable;

        public ExtendedListViewRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                UpdateSelectionMode();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ListView.SelectionMode))
            {
                UpdateSelectionMode();
            }
        }

        private void UpdateSelectionMode()
        {
            if (Element != null &&
                Control is Android.Widget.ListView droidListView)
            {
                switch (Element.SelectionMode)
                {
                    case ListViewSelectionMode.None:
                        _selectorDrawable = droidListView.Selector;
                        droidListView.Selector = new ColorDrawable(Android.Graphics.Color.Transparent);
                        break;
                    case ListViewSelectionMode.Single:
                        if (_selectorDrawable != null)
                        {
                            droidListView.Selector = _selectorDrawable;
                        }
                        break;
                }
            }
        }
    }
}
