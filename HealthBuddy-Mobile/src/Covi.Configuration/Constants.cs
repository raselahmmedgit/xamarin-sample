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

namespace Covi.Configuration
{
    public static class Constants
    {
        public static void Initialize(IEnvironmentConfiguration environmentConfiguration)
        {
            CacheExpirationConstants.MetadataExpirationTime = TimeSpan.FromMinutes(int.Parse(environmentConfiguration.GetValue("CacheExpirationConstants_MetadataExpirationTimeMinutes")));
        }

        public static class PlatformConstants
        {
            public static readonly string EndpointUrl = Configuration.Helpers.Secrets.EndpointUrl;
        }

        public static class AppCenterConstants
        {
            public static readonly string Secret_iOS = Configuration.Helpers.Secrets.AppCenter_iOS_Secret;
            public static readonly string Secret_Android = Configuration.Helpers.Secrets.AppCenter_Android_Secret;
        }

        public static class SecretsProviderConstants
        {
            public static readonly string EncryptionKeyFieldName = Configuration.Helpers.Secrets.SecretsProvider_EncryptionKeyFieldName;
            public static readonly string DeviceIdentifierFieldName = Configuration.Helpers.Secrets.SecretsProvider_DeviceIdentifierFieldName;
            public static readonly string InitializedFlagFieldName = Configuration.Helpers.Secrets.SecretsProvider_InitializedFlagFieldName;
            public static readonly string IsFirstRunFieldName = Configuration.Helpers.Secrets.SecretsProvider_IsFirstRunFieldName;
        }

        public static class PushNotificationsConstants
        {
            public static readonly string NotificationChannelName = Configuration.Helpers.Secrets.PushNotifications_NotificationChannelName;
        }

        public static class CacheExpirationConstants
        {
            public static TimeSpan MetadataExpirationTime { get; internal set; }
        }

        public static class HybridConstants
        {
            public static readonly string HybridEndpointUrl = Configuration.Helpers.Secrets.HybridEndpointUrl;
        }

        public static class PlatformAuthenticationConstants
        {
            public static readonly string PlatformSubscriptionKey = Configuration.Helpers.Secrets.PlatformSubscriptionKey;
        }
    }
}
