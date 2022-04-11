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
using Prism.AppModel;
using Prism.Navigation;
using Covi.Features.ComponentsManagement;

namespace Covi.Features
{
    /// <summary>
    /// Represents a component on the ui that is dependent on the host <see cref="CompositeViewModelBase"/> lifecycle.
    /// </summary>
    public interface IComponent : IPageLifecycleAware, IInitialize
    {
        /// <summary>
        /// Gets the identifier provided by <see cref="IComponentService"/>.
        /// </summary>
        string ComponentGroupKey { get; }

        /// <summary>
        /// Attach component to the host.
        /// </summary>
        /// <param name="hostContext">Host context.</param>
        /// <param name="componentGroupKey">Identifier of the component group.</param>
        IDisposable Attach(HostContext hostContext, string componentGroupKey);
    }
}
