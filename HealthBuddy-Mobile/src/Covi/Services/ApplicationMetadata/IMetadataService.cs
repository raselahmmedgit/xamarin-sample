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

using System.Threading;
using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;

namespace Covi.Services.ApplicationMetadata
{
    /// <summary>
    /// Provides operations to get application metadata.
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// Retrieves application metadata from the server if needed.
        /// </summary>
        /// <param name="forceUpdate">Whether to ignore cache or not.</param>
        /// <returns>Actual <see cref="Metadata"/>.</returns>
        Task<Metadata> FetchMetadataIfNeededAsync(bool forceUpdate = false);

        /// <summary>
        /// Safely retrieves application metadata from the server if needed.
        /// </summary>
        /// <param name="forceUpdate">Whether to ignore cache or not.</param>
        /// <returns>Actual <see cref="Metadata"/> if successful, otherwise <c>null</c>.</returns>
        Task<Metadata> TryFetchMetadataIfNeededAsync(bool forceUpdate = false);

        /// <summary>
        /// Returns cached application metadata. Will return <c>null</c> if there is no data.
        /// </summary>
        /// <returns>Returns cached application metadata.</returns>
        Task<Metadata> GetCachedMetadataAsync();

        Task SetMetadataAsync(Metadata metadata);
    }
}
