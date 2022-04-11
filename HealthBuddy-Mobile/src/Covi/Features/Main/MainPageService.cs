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
using System.Collections.Generic;
using System.Linq;
using Prism.Ioc;
using Xamarin.Forms;

namespace Covi.Features.Main
{
    public static class MainPageService
    {
        private static List<Type> _pageServiceProviders = new List<Type>();

        private static int DefaultPageIndex { get; set; }

        public static void SetResolver(Func<IContainerProvider> serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static void RegisterPageService<TPageProvider>()
            where TPageProvider : IMainPageProviderService
        {
            _pageServiceProviders.Add(typeof(TPageProvider));
        }

        public static IList<Page> GetPages()
        {
            var pageProviders = _pageServiceProviders
                .Select(ServiceProvider().Resolve)
                .Cast<IMainPageProviderService>()
                .ToList();

            var pages = pageProviders
                .Select((p, i) => new
                {
                    HasPage = p.TryGetPage(out var page, out var isDefault),
                    Page = page,
                    IsDefault = isDefault,
                    Index = i
                })
                .Where(r => r.HasPage)
                .Select(p =>
                {
                    if (p.IsDefault)
                    {
                        DefaultPageIndex = p.Index;
                    }

                    return p.Page;
                })
                .ToList();

            return pages;
        }

        public static int GetDefaultPageIndex()
        {
            return DefaultPageIndex;
        }

        private static Func<IContainerProvider> ServiceProvider { get; set; }
    }
}
