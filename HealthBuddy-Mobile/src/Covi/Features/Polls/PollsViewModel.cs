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
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Covi.Features.Account.Models;
using Covi.Features.Account.Services;
using Covi.Features.AccountFeaturesInformation;
using Covi.Features.Analytics;
using Covi.Features.Chat.Handlers;
using Covi.Features.Components.InfoView;
using Covi.Features.Controls.HybridWebView;
using Covi.Features.CreateProfile;
using Covi.Features.UserProfile.Services;
using Covi.Services.Http.UriHandlers;
using Covi.Services.Serialization;
using Covi.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Covi.Features.Polls
{
    public class PollsViewModel : ViewModelBase, IInteractionChannelHandler
    {
        private const string LogoImageSource = "resource://Covi.Features.Polls.Resources.Images.poll.svg";
        private const string ErrorImgSource = "connection_problem.svg";
        private const string InitPhrase = "polls";

        private readonly ILogger _logger;
        private readonly IMessagesProcessor _messagesProcessor;
        private readonly IMessageInteractor _messageInteractor;
        private readonly IAccountInformationContainer _accountContainer;
        private readonly IMediator _mediator;

        public bool ShowRestricted { get; private set; }
        public bool ShowPolls { get; private set; }

        public string PollsLogoImageSource => LogoImageSource;

        public IReadOnlyList<string> PollsInformationItemsList { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> CreateAccountCommand { get; private set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> LogInCommand { get; private set; }

        public string Uri { get; }
        public string InputText { get; set; }
        public bool IsInputVisible { get; private set; } = true;
        public Interaction<Message, bool> SendMessageInteraction { get; }
        public ICommand SendCommand { get; }
        public ICommand HandleMessageCommand { get; }
        public IInfoViewModel InfoViewModel { get; private set; }
        public IUriOpeningHandler UriOpeningHandler { get; }

        private bool IsOpenedAtLeastOnce { get; set; }
        private bool IsApiReady { get; set; }
        private bool IsInitMessageSent { get; set; }

        public PollsViewModel(
            ISerializer serializer,
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IMessagesProcessor messagesProcessor,
            IAccountInformationContainer accountContainer,
            IUserAccountContainer userAccountContainer)
        {
            _mediator = mediator;
            _messagesProcessor = messagesProcessor;
            _logger = loggerFactory.CreateLogger<PollsViewModel>();
            _accountContainer = accountContainer;

            Uri = Configuration.Constants.HybridConstants.HybridEndpointUrl;
            UriOpeningHandler = new HybridWebViewUriOpeningHandler(Uri);
            SendMessageInteraction = new Interaction<Message, bool>();
            _messageInteractor = new MessageInteractor(loggerFactory, serializer, userAccountContainer, SendMessageInteraction);

            _messagesProcessor.AddHandler(new InputMessageHandler(b => IsInputVisible = b));
            _messagesProcessor.AddHandler(new WebSessionConnectionHandler(loggerFactory, isConnected => IsBusy = !isConnected));

            HandleMessageCommand = ReactiveCommand.CreateFromTask<Message>(HandleMessageAsync);
            SendCommand = ReactiveCommand.CreateFromTask(SendAsync);

            IsBusy = true;
        }

        public async void ApiReadyMessage()
        {
            IsApiReady = true;
            await InitializeIfNeededAsync();
        }

        public override async void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(PollsViewModel));

            _accountContainer.Changes.Subscribe(HandleAccountChanged).DisposeWith(lifecycleDisposable);
            IsOpenedAtLeastOnce = true;

            await InitializeIfNeededAsync();
        }

        private async Task InitializeIfNeededAsync()
        {
            if (!IsOpenedAtLeastOnce)
            {
                return;
            }

            if (IsApiReady && !IsInitMessageSent)
            {
                await _messageInteractor.SendInit(InitPhrase);
                OnInitialized();
                IsInitMessageSent = true;
            }
        }

        private void HandleAccountChanged(AccountInformation obj)
        {
            if (obj != null && obj.Roles.Count > 0)
            {
                // Polls page
                ShowRestricted = false;
                ShowPolls = true;
            }
            else
            {
                // Polls restricted
                InitRestricted();

                ShowRestricted = true;
                ShowPolls = false;

                IsBusy = false;
            }
        }

        private void InitRestricted()
        {
            PollsInformationItemsList = new List<string>()
            {
                Resources.Localization.Polls_InfoItem1_Text,
                Resources.Localization.Polls_InfoItem2_Text
            };

            CreateAccountCommand = ReactiveCommand.CreateFromTask(HandleCreateAccountAsync);
            LogInCommand = ReactiveCommand.CreateFromTask(HandleLogInAsync);
        }

        private async Task HandleCreateAccountAsync()
        {
            try
            {
                IsBusy = true;
                await _mediator.Send(new CreateProfileAction());
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task HandleLogInAsync()
        {
            try
            {
                IsBusy = true;
                await _mediator.Send(new NavigateToLogInAction());
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SetErrorState()
        {
            InfoViewModel = InfoViewModelFactory.CreateViewModel(ErrorImgSource, Covi.Resources.Localization.Exception_NoInternetConnection);
            IsBusy = false;
        }

        private void OnInitialized()
        {
            IsBusy = false;
        }

        private async Task SendAsync()
        {
            await _messageInteractor.SendText(InputText);
            InputText = string.Empty;
        }

        private async Task HandleMessageAsync(Message message, CancellationToken ct = default)
        {
            await _messagesProcessor.HandleAsync(message);
        }
    }
}
