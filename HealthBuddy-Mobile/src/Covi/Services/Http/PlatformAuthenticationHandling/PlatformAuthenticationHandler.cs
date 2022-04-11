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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Covi.Configuration;

namespace Covi.Services.Http.PlatformAuthenticationHandling
{
    public class PlatformAuthenticationHandler : DelegatingHandler
    {
        private const string PlatformAuthenticationHeaderKey = "Ocp-Apim-Subscription-Key";
        private const string DevicePlatformInformationHeaderKey = "X-APP";
        private const string ApplicationBuildInformationHeaderKey = "X-APP-V";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(Constants.PlatformAuthenticationConstants.PlatformSubscriptionKey))
                {
                    request.AddHeaderSafely(PlatformAuthenticationHeaderKey, Constants.PlatformAuthenticationConstants.PlatformSubscriptionKey);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to add platform subscription key. Reason: " + e);
            }

            try
            {
                request.AddHeaderSafely(DevicePlatformInformationHeaderKey, new[] { Xamarin.Essentials.DeviceInfo.Platform.ToString().ToLowerInvariant() });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to get device platform information. Reason: " + e);
            }

            try
            {
                request.AddHeaderSafely(ApplicationBuildInformationHeaderKey, new[] { Xamarin.Essentials.AppInfo.BuildString });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to get application build information. Reason: " + e);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
