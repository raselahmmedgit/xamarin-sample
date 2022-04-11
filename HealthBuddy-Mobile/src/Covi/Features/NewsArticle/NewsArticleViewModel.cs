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
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;
using Covi.Features.Analytics;
using Covi.Features.Components.InfoView;
using Covi.Features.NewsArticle.Actions;
using Covi.Features.Newsfeed.Services;
using Covi.Services.ErrorHandlers;
using Covi.Services.Http.ApiExceptions;
using Covi.Services.Navigation;
using Covi.Utils;
using Prism.Navigation;
using ReactiveUI;
using Xamarin.Essentials;

namespace Covi.Features.NewsArticle
{
    public class NewsArticleViewModel : ViewModelBase
    {
        private const string ErrorImgSource = "connection_problem.svg";
        private readonly INewsfeedService _newsArticleService;
        private string _articleId;
        private readonly IErrorHandler _errorHandler;
        private string _languageCode;

        public string Title { get; private set; }
        public string Source { get; private set; }
        public string CreatedDate { get; private set; }
        public string ImageLink { get; private set; }
        public string Body { get; private set; }
        public string VideoLink { get; private set; }
        public bool HasError { get; private set; }
        public bool HasVideoContent => !string.IsNullOrEmpty(VideoLink);

        public IInfoViewModel InfoViewModel { get; private set; }

        public ReactiveCommand<Unit, Unit> OpenVideoLinkCommand { get; }

        public NewsArticleViewModel(INewsfeedService newsArticleService, IErrorHandler errorHandler)
        {
            _newsArticleService = newsArticleService;
            _errorHandler = errorHandler;
            OpenVideoLinkCommand = ReactiveCommand.CreateFromTask(OpenVideoLinkAsync);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InfoViewModel = InfoViewModelFactory.CreateViewModel(
                ErrorImgSource, string.Empty, async () => { await InitializeAsync(parameters); });
            InitializeAsync(parameters).FireAndForget();
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(NewsArticleViewModel));
        }

        private async Task InitializeAsync(INavigationParameters parameters)
        {
            try
            {
                HasError = false;
                IsBusy = true;

                if (parameters != null)
                {
                    var newsArticleParameters = await parameters.GetNavigationParametersAsync<NewsArticleParameters>();

                    if (newsArticleParameters != null)
                    {
                        SetBaseProperties(newsArticleParameters);
                    }
                }

                if (_articleId != null)
                {
                    var article = await _newsArticleService.GetArticleAsync(_articleId, _languageCode);
                    SetModel(article);
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

                    InfoViewModel.InformationText = e.Message;
                    HasError = true;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetModel(Article article)
        {
            Body = article.Body.Text;
            var date = article.Created;
            if (date != null)
            {
                CreatedDate = ((DateTime)date).ToLocalTime()
                    .ToString("dd MMM yyyy", CultureInfo.CurrentUICulture);
            }

            Title ??= article.Title;
            Source ??= article.Source;

            if (VideoLink == null && article.VideoLink != null)
            {
                VideoLink = article.VideoLink.Uri;
            }

            if (ImageLink == null && article.ImageLink != null)
            {
                ImageLink = article.ImageLink.Url;
            }
        }

        private void SetBaseProperties(NewsArticleParameters request)
        {
            _articleId = request.ArticleId;
            _languageCode = request.LanguageCode;
            VideoLink = request.VideoLink;
            ImageLink = request.ImageLink;
            Title = request.Title;
            Source = request.Source;
        }

        private async Task OpenVideoLinkAsync()
        {
            if (HasVideoContent)
                await Launcher.OpenAsync(new Uri(VideoLink));
        }
    }
}
