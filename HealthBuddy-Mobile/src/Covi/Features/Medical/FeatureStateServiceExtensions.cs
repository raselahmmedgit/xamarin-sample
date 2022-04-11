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

using Covi.Configuration;
using Covi.Services.Feature;

namespace Covi.Features.Medical
{
    public static class FeatureStateServiceExtensions
    {
        private const string MedicalFeatureRemoteConfigurationKey = "medicalLogin";
        private const string MedicalEnvironmentConfigurationKey = "FeatureConfiguration_IsMedicalEnabled";

        public static bool IsMedicalFeatureEnabled(this IFeatureStateService featureStateService, IEnvironmentConfiguration environmentConfiguration = null)
        {
            bool defaultValue = false;
            if (environmentConfiguration != null)
            {
                bool.TryParse(environmentConfiguration.GetValue(MedicalEnvironmentConfigurationKey), out defaultValue);
            }

            var value = AsyncHelpers.RunSync(() => featureStateService.GetValueAsync(MedicalFeatureRemoteConfigurationKey, string.Empty));

            if (bool.TryParse(value, out var result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}
