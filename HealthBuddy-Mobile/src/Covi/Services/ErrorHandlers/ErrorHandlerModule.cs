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

using Prism.Ioc;
using Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Covi.Services.ErrorHandlers
{
    public class ErrorHandlerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<DomainErrorHandler>();
            containerRegistry.Register<LastChanceErrorHandler>();
            containerRegistry.Register<LastChanceErrorHandler>();
            containerRegistry.Register<OperationCancelledErrorHandler>();
            containerRegistry.Register<MaintenanceErrorHandler>();
            containerRegistry.Register<ForceUpdateErrorHandler>();
            containerRegistry.Register<ExpiredTokenErrorHandler>();

            containerRegistry.RegisterSingletonFromDelegate<IErrorHandler>((provider) =>
            {
                var domainErrorHandler = provider.GetRequiredService<DomainErrorHandler>();
                var lastChanceErrorHandler = provider.GetRequiredService<LastChanceErrorHandler>();
                var operationCancelledErrorHandler = provider.GetRequiredService<OperationCancelledErrorHandler>();
                var maintenanceErrorHandler = provider.GetRequiredService<MaintenanceErrorHandler>();
                var forceUpdateErrorHandler = provider.GetRequiredService<ForceUpdateErrorHandler>();
                var expiredTokenErrorHandler = provider.GetRequiredService<ExpiredTokenErrorHandler>();

                // Order of parameters defines error handling order.
                return new CompositeErrorHandler(maintenanceErrorHandler,
                    forceUpdateErrorHandler,
                    expiredTokenErrorHandler,
                    operationCancelledErrorHandler,
                    domainErrorHandler,
                    lastChanceErrorHandler);
            });
        }
    }
}
