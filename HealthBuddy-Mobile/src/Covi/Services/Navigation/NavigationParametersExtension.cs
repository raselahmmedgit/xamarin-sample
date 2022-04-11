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
using Covi.Services.Serialization;
using Prism.Navigation;

namespace Covi.Services.Navigation
{
    public static class NavigationParametersExtension
    {
        private const string Key = "NavParams";

        public static async Task<NavigationParameters> ToNavigationParametersAsync<T>(this T parameters)
            where T : NavigationParametersBase
        {
            var serializeNavigationParams = await Serializer.Instance.SerializeAsync(parameters);
            var result = new NavigationParameters { { Key, serializeNavigationParams } };
            return result;
        }

        public static async Task<T> GetNavigationParametersAsync<T>(this INavigationParameters parameters)
            where T : NavigationParametersBase
        {
            var result = default(T);

            if (parameters.TryGetValue<string>(Key, out var payload))
            {
                result = await Serializer.Instance.DeserializeAsync<T>(payload);
            }

            return result;
        }
    }
}
