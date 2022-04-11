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
using Covi.Features.Analytics;
using Microsoft.Extensions.Logging;

namespace Covi.iOS.Features.Analytics
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
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
                Firebase.Analytics.Analytics.SetScreenNameAndClass(name, null);
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
                    Firebase.Analytics.Analytics.LogEvent(name, parameters: null);
                    return;
                }

                Firebase.Analytics.Analytics.LogEvent(name, (Dictionary<object, object>)parameters);
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
                Firebase.Core.App.Configure();
                _initialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Firebase analytics");
            }
        }
    }
}
