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
using MediatR;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Covi.Features.Rumours
{
    public class RumoursViewModel : ViewModelBase, IInteractionChannelHandler
    {
        private const string LogoImageSource = "resource://Covi.Features.Rumours.Resources.Images.rumours_logo.svg";
        private const string ErrorImgSource = "connection_problem.svg";
        private const string InitPhrase = "reportrumors";
        private const string InitPrefix = "rumors";

        private readonly IMessageInteractor _messageInteractor;
        private readonly IAccountInformationContainer _accountContainer;
        private readonly IMediator _mediator;
        private readonly IMessagesProcessor _messagesProcessor;

        public bool ShowRestricted { get; private set; }

        public bool ShowRumours { get; private set; }

        #region RestrictedRumoursProperties

        public string RumoursLogoImageSource { get; private set; }

        public IReadOnlyList<string> RumoursInformationItemsList { get; set; }

        public ReactiveCommand<Unit, Unit> CreateAccountCommand { get; private set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> LogInCommand { get; private set; }

        #endregion

        #region AllowedRumoursProperties

        public string Uri { get; }

        public string InputText { get; set; }

        public bool IsInputVisible { get; private set; } = true;

        public Interaction<Message, bool> SendMessageInteraction { get; }

        public ICommand SendCommand { get; private set; }

        public ICommand HandleMessageCommand { get; private set; }

        public IInfoViewModel InfoViewModel { get; private set; }

        public IUriOpeningHandler UriOpeningHandler { get; }

        #endregion

        public RumoursViewModel(
            IMediator mediator,
            ISerializer serializer,
            ILoggerFactory loggerFactory,
            IMessagesProcessor messagesProcessor,
            IAccountInformationContainer accountContainer,
            IUserAccountContainer userAccountContainer)
        {
            _mediator = mediator;
            _accountContainer = accountContainer;
            _messagesProcessor = messagesProcessor;

            Uri = Configuration.Constants.HybridConstants.HybridEndpointUrl;
            UriOpeningHandler = new HybridWebViewUriOpeningHandler(Uri);
            SendMessageInteraction = new Interaction<Message, bool>();
            _messageInteractor = new MessageInteractor(loggerFactory, serializer, userAccountContainer, SendMessageInteraction);

            _messagesProcessor.AddHandler(new InputMessageHandler(b => IsInputVisible = b));
            _messagesProcessor.AddHandler(new WebSessionConnectionHandler(loggerFactory, isConnected => IsBusy = !isConnected));

            IsBusy = true;
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(RumoursViewModel));

            _accountContainer.Changes.Subscribe(HandleAccountChanged).DisposeWith(lifecycleDisposable);
        }

        private void HandleAccountChanged(AccountInformation obj)
        {
            if (obj != null && obj.Roles.Count > 0)
            {
                InitRumours();

                ShowRestricted = false;
                ShowRumours = true;
            }
            else
            {
                InitRestricted();

                ShowRestricted = true;
                ShowRumours = false;
            }
        }

        public async void ApiReadyMessage()
        {
            await _messageInteractor.SendInit(InitPhrase, InitPrefix);
            OnInitialized();
        }

        private void OnInitialized()
        {
            IsBusy = false;
        }

        public void SetErrorState()
        {
            InfoViewModel = InfoViewModelFactory.CreateViewModel(ErrorImgSource, Covi.Resources.Localization.Exception_NoInternetConnection);
            IsBusy = false;
        }

        private void InitRumours()
        {
            HandleMessageCommand = ReactiveCommand.CreateFromTask<Message>(HandleMessageAsync);
            SendCommand = ReactiveCommand.CreateFromTask(SendAsync);
        }

        private void InitRestricted()
        {
            RumoursLogoImageSource = LogoImageSource;
            RumoursInformationItemsList = new List<string>()
            {
                Rumours.Resources.Localization.Rumours_InfoItem1_Text,
                Rumours.Resources.Localization.Rumours_InfoItem2_Text
            };

            CreateAccountCommand = ReactiveCommand.CreateFromTask(HandleCreateAccountAsync);
            LogInCommand = ReactiveCommand.CreateFromTask(HandleLogInAsync);
            IsBusy = false;
        }

        #region ResctrictedMethods

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

        #endregion

        #region AllowedRumoursMethods

        private async Task SendAsync()
        {
            await _messageInteractor.SendText(InputText);
            InputText = string.Empty;
        }

        private async Task HandleMessageAsync(Message message, CancellationToken ct = default)
        {
            await _messagesProcessor.HandleAsync(message);
        }

        #endregion

    }
}
