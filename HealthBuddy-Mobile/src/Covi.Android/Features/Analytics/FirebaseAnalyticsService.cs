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
using Android.OS;
using Covi.Features.Analytics;
using Firebase.Analytics;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace Covi.Droid.Features.Analytics
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        private FirebaseAnalytics _instance;
        private readonly ILogger _logger;
        private bool _initialized;

        public FirebaseAnalyticsService()
        {
            _logger = Covi.Logs.Logger.Get(this);

            Init();
        }

        public void LogScreen(string name)
        {
            if (!_initialized || string.IsNullOrEmpty(name))
            {
                return;
            }

            try
            {
                _instance.SetCurrentScreen(Platform.CurrentActivity, name, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log screen through Firebase analytics");
            }
        }

        public void LogEvent(string name, IDictionary<string, string> parameters)
        {
            if (!_initialized || string.IsNullOrEmpty(name))
            {
                return;
            }

            try
            {
                if (parameters == null)
                {
                    _instance.LogEvent(name, null);
                    return;
                }

                var bundle = new Bundle();

                foreach (var item in parameters)
                {
                    bundle.PutString(item.Key, item.Value);
                }

                _instance.LogEvent(name, bundle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log event through Firebase analytics");
            }
        }

        private void Init()
        {
            try
            {
                _instance = FirebaseAnalytics.GetInstance(Android.App.Application.Context);
                _initialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Firebase analytics");
            }
        }
    }
}
