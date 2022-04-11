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

using Covi.Client.Services.Platform.Models;

namespace Covi.Features.Newsfeed.Components.News
{
    public class NewsState
    {
        public static NewsState CreateDefaultState()
        {
            return new NewsState();
        }

        public static NewsState CreateBusyState()
        {
            return new NewsState()
            {
                State = NewsContentState.Busy
            };
        }

        public static NewsState CreateErrorState(string errorMessage)
        {
            return new NewsState()
            {
                State = NewsContentState.Error,
                ErrorMessage = errorMessage
            };
        }

        public static NewsState CreateEmptyState()
        {
            return new NewsState()
            {
                State = NewsContentState.Empty
            };
        }

        public static NewsState CreateNewsState(NewsfeedArticles newsfeedArticles)
        {
            return new NewsState()
            {
                State = NewsContentState.News,
                NewsfeedArticlesList = newsfeedArticles
            };
        }

        public NewsState()
        {
            State = NewsContentState.Default;
        }

        public NewsContentState State { get; private set; }

        public NewsfeedArticles NewsfeedArticlesList { get; private set; }

        public string ErrorMessage { get; private set; }
    }

    public enum NewsContentState
    {
        Default,
        Busy,
        News,
        Empty,
        Error
    }
}
