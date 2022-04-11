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

using System.Threading.Tasks;
using Covi.Services.Security.SecureVault;
using Covi.Services.Serialization;

namespace Covi.Services.Http.SessionContainer
{
    public class SessionContainer : ISessionContainer
    {
        private readonly ISecureVault _secureVault;
        private readonly ISerializer _serializer;
        private const string SessionInfoKey = "SessionInfoKey";

        public SessionContainer(
            ISecureVault secureVault,
            ISerializer serializer)
        {
            _secureVault = secureVault;
            _serializer = serializer;
        }

        public async Task SetAsync(SessionInfo sessionInfo)
        {
            if (sessionInfo == null)
            {
                await _secureVault.RemoveAsync(SessionInfoKey).ConfigureAwait(false);
            }

            var serializedString = await _serializer.SerializeAsync(sessionInfo).ConfigureAwait(false);
            await _secureVault.SetAsync(SessionInfoKey, serializedString).ConfigureAwait(false);
        }

        public async Task<SessionInfo> GetAsync()
        {
            try
            {
                var serializedString = await _secureVault.GetAsync(SessionInfoKey).ConfigureAwait(false);
                if (string.IsNullOrEmpty(serializedString))
                {
                    return null;
                }

                var sessionInfo = await _serializer.DeserializeAsync<SessionInfo>(serializedString).ConfigureAwait(false);
                return sessionInfo;
            }
            catch
            {
                return null;
            }
        }
    }
}
