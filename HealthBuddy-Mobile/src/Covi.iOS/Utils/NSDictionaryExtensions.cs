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
using System.Linq;
using Foundation;

namespace Covi.iOS
{
    public static class NSDictionaryExtensions
    {
        public static Dictionary<string, string> ConvertToDictionary(this NSDictionary nsDictionary)
        {
            if (nsDictionary == null)
            {
                return null;
            }

            return nsDictionary.ToDictionary<KeyValuePair<NSObject, NSObject>, string, string>(
                item => (NSString)item.Key, item => item.Value.ToString());
        }

        public static int GetIntValue(this NSDictionary nsDictionary, string key, int defaultValue = 0)
        {
            if (nsDictionary != null && nsDictionary.TryGetValue((NSString)key, out var value) && value is NSNumber number)
            {
                return number.Int32Value;
            }

            return defaultValue;
        }

        public static string GetStringValue(this NSDictionary nsDictionary, string key)
        {
            if (nsDictionary != null && nsDictionary.TryGetValue((NSString)key, out var value))
            {
                return value?.ToString();
            }

            return null;
        }
    }
}
