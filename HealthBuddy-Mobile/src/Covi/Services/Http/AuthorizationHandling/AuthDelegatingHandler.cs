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

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Covi.Services.Http.SessionContainer;

namespace Covi.Services.Http.AuthorizationHandling
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private const string Bearer = "Bearer";
        private readonly ISessionContainer _sessionInfoContainer;

        public AuthDelegatingHandler(ISessionContainer sessionInfoContainer)
        {
            _sessionInfoContainer = sessionInfoContainer;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authToken = await GetAuthTokenAsync().ConfigureAwait(false);
            ApplyAuthenticationToken(request, authToken);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static void ApplyAuthenticationToken(HttpRequestMessage request, string authToken)
        {
            if (!string.IsNullOrEmpty(authToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(Bearer, authToken);
            }
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var sessionInfo = await _sessionInfoContainer.GetAsync().ConfigureAwait(false);
            return sessionInfo?.AccessToken;
        }
    }
}
