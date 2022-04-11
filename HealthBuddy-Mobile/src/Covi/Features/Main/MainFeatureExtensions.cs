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
using DryIoc;
using Prism.DryIoc.Extensions;
using Prism.Ioc;

namespace Covi.Features.Main
{
    public static class MainFeatureExtensions
    {
        public static MainFeature RegisterPageService<TPageService>(this MainFeature feature, IContainerProvider containerProvider)
            where TPageService : IMainPageProviderService
        {
            var containerRegistry = containerProvider.GetContainer();

            containerRegistry.Register<TPageService>(Reuse.Singleton);
            MainPageService.RegisterPageService<TPageService>();

            return feature;
        }
    }
}
