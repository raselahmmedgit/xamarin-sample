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

namespace Covi.Features.Main
{
    /// <summary>
    /// Page items which are implementing this interface are going to be ignored during navigation process in the context of
    /// the <see cref="Xamarin.Forms.TabbedPage"/> (its specific implementation <see cref="MainPage"/>).
    /// </summary>
    public interface ITabbedPageItemCustomHandler
    {
        /// <summary>
        /// This method will be called when navigation to this page is attempted via <see cref="Xamarin.Forms.TabbedPage"/>.
        /// </summary>
        void HandleTabbedPageItemNavigation();
    }
}
