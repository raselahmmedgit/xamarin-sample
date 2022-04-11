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
using System.Threading.Tasks;

namespace Covi.Features.Controls.HybridWebView
{
    public class HybridWebView : Xamarin.Forms.WebView
    {
        private const string DispatchWebCommand = @"window[""dispatchWeb""]";

        public void Cleanup()
        {
            React = null;
        }

        public void ReactAction(string data)
        {
            React?.Invoke(data);
        }

        public Action<string> React { get; set; }

        public async Task<bool> SendMessageAsync(string message)
        {
            var result = await EvaluateJavaScriptAsync($"{DispatchWebCommand}('{message}');");
            return true;
        }
    }
}
