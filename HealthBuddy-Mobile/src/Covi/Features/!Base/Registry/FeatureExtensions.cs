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

namespace Covi.Features.Registry
{
    /// <summary>
    /// Extensions for the feature initialization setup.
    /// </summary>
    public static class FeatureExtensions
    {
        /// <summary>
        /// Registers navigation action handlers (triggers).
        /// Note: this method is named in such way for better readability of the trigger registration process.
        /// </summary>
        /// <typeparam name="TFeature">Type of the feature to initialize.</typeparam>
        /// <param name="feature">Feature to initialize.</param>
        /// <param name="containerProvider"><see cref="IContainerProvider"/> for types registration.</param>
        /// <param name="actionHandlerTypes">Navigation action handlers to register.</param>
        /// <returns>Feature to initialize for chaining.</returns>
        public static TFeature RegisterNavigationTriggers<TFeature>(this TFeature feature, IContainerProvider containerProvider, params Type[] actionHandlerTypes)
            where TFeature : IFeature, new()
        {
            var container = containerProvider.GetContainer();
            container.RegisterMany(actionHandlerTypes);

            return feature;
        }

        /// <summary>
        /// Registers any general purpose action handlers (triggers).
        /// Note: this method is named in such way for better readability of the trigger registration process.
        /// </summary>
        /// <typeparam name="TFeature">Type of the feature to initialize.</typeparam>
        /// <param name="feature">Feature to initialize.</param>
        /// <param name="containerProvider"><see cref="IContainerProvider"/> for types registration.</param>
        /// <param name="actionHandlerTypes">Action handlers to register.</param>
        /// <returns>Feature to initialize for chaining.</returns>
        public static TFeature RegisterGeneralPurposeTriggers<TFeature>(this TFeature feature, IContainerProvider containerProvider, params Type[] actionHandlerTypes)
            where TFeature : IFeature, new()
        {
            var container = containerProvider.GetContainer();
            container.RegisterMany(actionHandlerTypes);

            return feature;
        }
    }
}
