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
using System.Threading;
using System.Threading.Tasks;
using Covi.Features.Analytics;
using Covi.Features.Components.BusyIndicator;
using Covi.Features.Components.InfoView;
using Covi.Features.ComponentsManagement;
using Covi.Features.Newsfeed.Services;
using Covi.Features.UserState.Services;
using Covi.Services.ErrorHandlers;
using Covi.Services.Http.ApiExceptions;
using MediatR;

namespace Covi.Features.Newsfeed.Components.News
{
    public class NewsComponentService : StatefulComponentServiceBase<NewsState>
    {
        private const string ErrorImgSource = "connection_problem.svg";
        private const string NoContentImgSource = "no_content.svg";

        private CancellationTokenSource _lifecycleCancellationTokenSource = new CancellationTokenSource();
        private readonly IUserStatusContainer _userStatusContainer;
        private readonly IErrorHandler _errorHandler;
        private readonly INewsfeedService _newsFeedService;
        private readonly IMediator _mediator;

        public NewsComponentService(
            IUserStatusContainer userStatusContainer,
            IErrorHandler errorHandler,
            IMediator mediator,
            INewsfeedService newsFeedService)
        {
            _userStatusContainer = userStatusContainer;
            _errorHandler = errorHandler;
            _mediator = mediator;
            _newsFeedService = newsFeedService;
        }

        public override string ComponentKey => nameof(NewsComponentService);

        public CancellationToken LifecycleToken => _lifecycleCancellationTokenSource.Token;

        public CancellationTokenSource LifecycleCancellationTokenSource => _lifecycleCancellationTokenSource;

        protected override IList<IComponent> UpdateState(NewsState state)
        {
            if (state == null)
            {
                return new List<IComponent>();
            }

            var result = new List<IComponent>();

            switch (state.State)
            {
                case NewsContentState.Busy:
                    result.Add(new BusyIndicatorViewModel());
                    break;
                case NewsContentState.Empty:
                    result.Add(GetFilterViewModel(state));
                    result.Add(GetInfoViewModel(state));
                    break;
                case NewsContentState.Error:
                    result.Add(GetFilterViewModel(state));
                    result.Add(GetInfoViewModel(state));
                    break;
                case NewsContentState.News:
                    result.Add(GetFilterViewModel(state));
                    result.AddRange(GetNewsItems(state));
                    break;
                case NewsContentState.Default:

                    break;
            }

            return result;
        }

        private NewsFilterViewModel GetFilterViewModel(NewsState state)
        {
            var result = new NewsFilterViewModel(_mediator);
            result.SetInfo(state);
            return result;
        }

        private InfoComponentViewModel GetInfoViewModel(NewsState state)
        {
            IInfoViewModel viewModel;

            if (state.State == NewsContentState.Empty)
            {
                viewModel = InfoViewModelFactory.CreateComponentViewModel(NoContentImgSource, Resources.Localization.Newsfeed_NoItems_Text);
            }
            else
            {
                var errorText = !string.IsNullOrEmpty(state.ErrorMessage)
                    ? state.ErrorMessage
                    : Covi.Resources.Localization.Component_Info_SomethingWrong_Text;

                viewModel = InfoViewModelFactory.CreateComponentViewModel(ErrorImgSource, errorText, async () => { await GetArticlesAsync(); });
            }

            return (InfoComponentViewModel)viewModel;
        }

        private IList<IComponent> GetNewsItems(NewsState state)
        {
            var result = new List<IComponent>();
            foreach (var newsItem in state.NewsfeedArticlesList.Data)
            {
                var viewModel = new NewsArticleItemViewModel(_mediator, newsItem);
                result.Add(viewModel);
            }

            return result;
        }

        protected override async void OnActivated()
        {
            base.OnActivated();

            Interlocked.Exchange(ref _lifecycleCancellationTokenSource, new CancellationTokenSource())?.Dispose();

            await GetArticlesAsync();
        }

        private async Task GetArticlesAsync()
        {
            try
            {
                var newsFeedTask = _newsFeedService.GetArticleListAsync(LifecycleToken);

                // If articles are received before timeout then don't use busy indicator.
                await newsFeedTask.ReturnInTimeoutAsync(default).ConfigureAwait(false);

                if (!newsFeedTask.IsCompleted)
                {
                    SetState(NewsState.CreateBusyState());
                }

                var articles = await newsFeedTask.ConfigureAwait(false);

                if (articles?.Data == null || articles.Data.Count == 0)
                {
                    SetState(NewsState.CreateEmptyState());
                }
                else
                {
                    SetState(NewsState.CreateNewsState(articles));
                }
            }
            catch (Exception e)
            {
                if (e is ICriticalException)
                {
                    await _errorHandler.HandleAsync(e);
                }
                else
                {
                    SetState(NewsState.CreateErrorState(e.Message));
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            _lifecycleCancellationTokenSource?.Cancel();
        }
    }
}
