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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Covi.Configuration;
using Covi.Features.AppSettings;

namespace Covi.Services.Localization
{
    public sealed class LocalizationService : ILocalizationService
    {
        private const string LocalizationServiceKey = nameof(LocalizationServiceKey);

        public IReadOnlyList<CultureInfo> AvailableCultures { get; }

        public CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

        public bool IsInitialized { get; private set; }

        public LocalizationService(IEnvironmentConfiguration environmentConfiguration)
        {
            var cultureCodes = environmentConfiguration.GetConfiguredCultures();

            // Initialize the list of available cultures. It should contain only the supported languages by the application.
            AvailableCultures = cultureCodes.SelectMany(code => TryGetCulture(code)).ToArray();
        }

        private IEnumerable<CultureInfo> TryGetCulture(string code)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(code.Trim());

                return new[] { culture };
            }
            catch(Exception ex)
            {
                return Enumerable.Empty<CultureInfo>();
            }
        }

        public void InitializeLocale()
        {
            var localeKey = AppPreferences.Instance.GetValue(LocalizationServiceKey);
            if (!string.IsNullOrWhiteSpace(localeKey))
            {
                var selectedCulture = AvailableCultures.FirstOrDefault(c => string.Equals(c.TwoLetterISOLanguageName, localeKey));
                if (selectedCulture != null)
                {
                    CultureInfo.DefaultThreadCurrentCulture = selectedCulture;
                    CultureInfo.DefaultThreadCurrentUICulture = selectedCulture;
                    CultureInfo.CurrentUICulture = selectedCulture;
                    CultureInfo.CurrentCulture = selectedCulture;
                }
            }

            IsInitialized = true;
        }

        public Task SaveSelectedCultureAsync(CultureInfo cultureInfo)
        {
            if (AvailableCultures.Contains(cultureInfo))
            {
                AppPreferences.Instance.SetValue(LocalizationServiceKey, cultureInfo.TwoLetterISOLanguageName);
            }

            return Task.CompletedTask;
        }
    }
}
