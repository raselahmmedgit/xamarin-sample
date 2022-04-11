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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;
using Covi.Features.Filters;
using Covi.Features.Filters.Dialogs;

namespace Covi.Features.Newsfeed.Services
{
    /// <summary>
    /// Provides operations to retrieve newsfeed.
    /// </summary>
    public interface INewsfeedService
    {
        /// <summary>
        /// Loads article list.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="NewsfeedList"/> result.</returns>
        Task<NewsfeedArticles> GetArticleListAsync(CancellationToken cancellationToken = default);

        Task<Article> GetArticleAsync(string articleId, string languageCode = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads current user filter settings or default if they are not present.
        /// </summary>
        /// <returns>List of current filter settings.</returns>
        Task<IList<FilterDescription>> GetFiltersAsync();

        /// <summary>
        /// Preliminary updates filter option item. Changes can be saved later by <see cref="CommitFilterChangesAsync"/> method.
        /// </summary>
        /// <param name="filterOption">Filter option item to update.</param>
        /// <returns>Current list of filter settings.</returns>
        IList<FilterDescription> SetFilterOption(FilterOptionItem filterOption);

        /// <summary>
        /// Reverts all uncommited filter changes to the last saved version.
        /// </summary>
        /// <returns><see cref="Task"/> to await.</returns>
        Task<IList<FilterDescription>> ResetToLastSavedAsync();

        /// <summary>
        /// Reverts all uncommited filter changes to the defaults.
        /// </summary>
        /// <returns><see cref="Task"/> to await.</returns>
        Task<IList<FilterDescription>> ResetToDefaultsAsync();

        /// <summary>
        /// Commits and saves filter changes 
        /// </summary>
        /// <returns><see cref="Task"/> to await.</returns>
        Task CommitFilterChangesAsync();
    }
}
