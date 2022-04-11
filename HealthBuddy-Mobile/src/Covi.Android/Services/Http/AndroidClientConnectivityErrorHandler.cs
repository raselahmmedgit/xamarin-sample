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

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Covi.Services.Http.ApiExceptions;

namespace Covi.Droid.Services.Http
{
    public class AndroidClientConnectivityErrorHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Java.IO.IOException ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    var operationCancelledException = new OperationCanceledException("Operation was cancelled", ex, cancellationToken);
                    operationCancelledException.Rethrow();
                }

                if (ex is Java.Net.UnknownHostException)
                {
                    var connectivityException = new ConnectivityException(ex);
                    connectivityException.Rethrow();
                }

                throw;
            }
        }
    }
}
