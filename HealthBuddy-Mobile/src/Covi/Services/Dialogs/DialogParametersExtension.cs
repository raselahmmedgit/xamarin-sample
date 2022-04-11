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

using Covi.Services.Serialization;
using Prism.Navigation;
using System.Threading.Tasks;

namespace Covi.Services.Dialogs
{
    public static class DialogParametersExtension
    {
        private const string PayloadKey = "Payload";

        public static async Task<NavigationParameters> ToDialogNavigationParametersAsync<T>(this T parameters)
            where T : DialogParametersBase
        {
            var serializePayload = await Serializer.Instance.SerializeAsync(parameters);
            var result = new NavigationParameters { { PayloadKey, serializePayload } };
            return result;
        }

        public static async Task<T> FromDialogNavigationParametersAsync<T>(this INavigationParameters parameters)
            where T : DialogParametersBase
        {
            var result = default(T);

            if (parameters.TryGetValue<string>(PayloadKey, out var payload))
            {
                result = await Serializer.Instance.DeserializeAsync<T>(payload);
            }

            return result;
        }

        public static async Task<Prism.Services.Dialogs.IDialogParameters> ToDialogParametersAsync<T>(this T parameters)
            where T : DialogParametersBase
        {
            var serializePayload = await Serializer.Instance.SerializeAsync(parameters);
            var result = new Prism.Services.Dialogs.DialogParameters()
            {
                { PayloadKey, serializePayload }
            };

            return result;
        }

        public static async Task<T> FromDialogParametersAsync<T>(this Prism.Services.Dialogs.IDialogParameters parameters)
            where T : DialogParametersBase
        {
            var result = default(T);

            if (parameters.TryGetValue<string>(PayloadKey, out var payload))
            {
                result = await Serializer.Instance.DeserializeAsync<T>(payload);
            }

            return result;
        }
    }
}
