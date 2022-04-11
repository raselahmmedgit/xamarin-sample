﻿// =========================================================================
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
using Covi.Services.Http.ApiExceptions;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;

namespace Covi.Droid.Services.Log
{
    public class AppCenterLogProvider : Microsoft.Extensions.Logging.ILoggerProvider
    {
        private AppCenterLogger _logger;
        private LogLevel _minLogLevel;

        public AppCenterLogProvider(LogLevel minLogLevel)
        {
            _minLogLevel = minLogLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger ??= new AppCenterLogger(_minLogLevel);
        }

        public void Dispose()
        {
            _logger = null;
        }

        private class AppCenterLogger : ILogger
        {
            private LogLevel _minLogLevel;

            public AppCenterLogger(LogLevel minLogLevel)
            {
                _minLogLevel = minLogLevel;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= _minLogLevel
                    && logLevel != LogLevel.None;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                var message = formatter(state, exception);
                var props = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(message))
                {
                    props["message"] = message;
                }
                System.Diagnostics.Debug.WriteLine(message);
                if (!string.IsNullOrEmpty(message) || exception != null)
                {
                    System.Console.WriteLine(message);
                    try
                    {
                        if (!(logLevel == LogLevel.Error))
                        {
                            Analytics.TrackEvent(logLevel.ToString(), props);
                        }
                        else if (!(exception is ApiResponseException || exception is ConnectivityException))
                        {
                            Crashes.TrackError(exception, props);
                        }
                    }
                    catch (Exception)
                    {
                        // ignore errors
                    }
                }
            }
        }
    }
}
