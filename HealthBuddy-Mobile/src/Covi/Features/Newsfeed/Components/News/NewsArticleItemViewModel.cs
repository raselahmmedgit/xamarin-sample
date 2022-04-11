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

using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;
using Covi.Features.NewsArticle.Actions;
using Covi.Utils.ReactiveCommandHelpers;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Covi.Features.Newsfeed.Components.News
{
    public class NewsArticleItemViewModel : ComponentViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly ShortArticle _articleModel;

        public string Id => _articleModel.Id;
        public string LanguageCode => _articleModel.LanguageCode;
        public string Title => _articleModel.Title;
        public string Source => _articleModel.Source;
        public string Summary => _articleModel.Body.Summary;
        public string ImageSource => _articleModel.ImageLink?.Url;
        public string VideoLink => _articleModel.VideoLink?.Uri;
        public bool HasVideoContent => !string.IsNullOrEmpty(VideoLink);

        public ReactiveCommand<Unit, Unit> ShowNewsArticleCommand { get; }

        public NewsArticleItemViewModel(IMediator mediator, ShortArticle model)
        {
            _mediator = mediator;
            _articleModel = model;
            ShowNewsArticleCommand = ReactiveCommandFactory.CreateLockedCommand(ShowNewsArticleAsync, nameof(NewsArticleItemViewModel));
        }

        private async Task ShowNewsArticleAsync()
        {
            var parameters = new NewsArticleParameters()
            {
                ArticleId = Id,
                LanguageCode = LanguageCode,
                Title = Title,
                Source = Source,
                ImageLink = ImageSource,
                VideoLink = VideoLink
            };

            await _mediator.Send(new NewsArticleAction(parameters));
        }
    }
}
