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
using Covi.Features.Main;
using Xamarin.Forms;

namespace Covi.Features.Polls
{
    public class PollsPageProviderService : IMainPageProviderService
    {
        private bool _isDefault;

        public Type PageType { get; } = typeof(PollsPage);

        public bool TryGetPage(out Page page, out bool isDefault)
        {
            page = new PollsPage();
            isDefault = _isDefault;
            return true;
        }

        public void SetDefault(bool isDefault)
        {
            _isDefault = isDefault;
        }
    }
}
