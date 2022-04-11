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
using Covi.Features.Analytics;
using Covi.Features.Chat.Data;
using Covi.Features.Chat.Handlers;
using Covi.Features.Chat.Components;
using Covi.Features.Components.InfoView;
using Covi.Features.Controls.HybridWebView;
using Covi.Features.RapidProFcmPushNotifications;
using Covi.Features.RapidProFcmPushNotifications.Services;
using Covi.Features.UserProfile.Services;
using Covi.Services.Http.UriHandlers;
using Covi.Services.Navigation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Covi.Services.Notification;
using Covi.Features.ChangeCountryProgram.Routes;
using System.Linq;

namespace Covi.Features.Chat
{
    public class ChatViewModel : ViewModelBase, IInteractionChannelHandler
    {
        private const string ErrorImgSource = "connection_problem.svg";
        private const string InitPhrase = "chat";
        private readonly IMessagesProcessor _messagesProcessor;
        private readonly ILogger _logger;
        //private readonly IMessageInteractor _messageInteractor;
        private readonly IUserAccountContainer _userAccountContainer;
        private readonly INotificationManager _notificationManager;
        private readonly IChangeCountryProgramRoute _changeCountryProgramRoute;

        private readonly INavigationServiceDelegate _navigationServiceDelegate;

        private readonly RapidProContainer _rapidProContainer;
        private readonly RapidProService _rapidProService;
        private readonly FirebaseContainer _firebaseContainer;

        private readonly ChatDatabase _chatDatabase;

        public string RapidProInitPhrase = "riseup";

        public bool ShowScrollTap { get; set; } = false;
        //public bool LastMessageVisible { get; set; } = true;

        public ObservableCollection<RapidProMessage> RapidProMessages { get; set; } = new ObservableCollection<RapidProMessage>();
        public string InputText { get; set; }
        public string RapidProMessageId { get; set; }
        public string ActionInputText { get; set; }
        public UserAccountInfo _userAccountInfo { get; set; }

        public ICommand OnSendCommand { get; set; }
        public ICommand OnActionSendCommand { get; set; }
        public ICommand RapidProMessageAppearingCommand { get; set; }
        public ICommand RapidProMessageDisappearingCommand { get; set; }

        //public ChatViewModel(
        //    ISerializer serializer,
        //    ILoggerFactory loggerFactory,
        //    IMessagesProcessor messagesProcessor,
        //    INavigationServiceDelegate navigationServiceDelegate,
        //    IUserAccountContainer userAccountContainer)
        //{
        //    _messagesProcessor = messagesProcessor;
        //    _logger = loggerFactory.CreateLogger<ChatViewModel>();
        //    _navigationServiceDelegate = navigationServiceDelegate;
        //    Uri = Configuration.Constants.HybridConstants.HybridEndpointUrl;
        //    SendMessageInteraction = new Interaction<Message, bool>();
        //    //_messageInteractor = new MessageInteractor(loggerFactory, serializer, userAccountContainer, SendMessageInteraction);

        //    UriOpeningHandler = new HybridWebViewUriOpeningHandler(Uri);

        //    _messagesProcessor.AddHandler(new InputMessageHandler(b => IsInputVisible = b));

        //    _messagesProcessor.AddHandler(new InputMessageHandler(b => IsInputVisible = b));
        //    _messagesProcessor.AddHandler(new WebSessionConnectionHandler(loggerFactory, isConnected => IsBusy = !isConnected));

        //    HandleMessageCommand = ReactiveCommand.CreateFromTask<Message>(HandleMessageAsync);
        //    SendCommand = ReactiveCommand.CreateFromTask(SendAsync);
        //    NavigateBackCommand = ReactiveCommand.CreateFromTask(NavigateBackAsync);
        //    IsBusy = true;

        //    _rapidProContainer = new RapidProContainer();
        //    _rapidProService = new RapidProService();
        //}

        public ChatViewModel(
            ILoggerFactory loggerFactory,
            IMessagesProcessor messagesProcessor,
            IUserAccountContainer userAccountContainer,
            INotificationManager notificationManager,
            IChangeCountryProgramRoute changeCountryProgramRoute,
            INavigationServiceDelegate navigationServiceDelegate)
        {
            _messagesProcessor = messagesProcessor;
            _logger = loggerFactory.CreateLogger<ChatViewModel>();
            _notificationManager = notificationManager;
            _changeCountryProgramRoute = changeCountryProgramRoute;
            _navigationServiceDelegate = navigationServiceDelegate;
            //Uri = Configuration.Constants.HybridConstants.HybridEndpointUrl;
            SendMessageInteraction = new Interaction<Message, bool>();

            //UriOpeningHandler = new HybridWebViewUriOpeningHandler(Uri);

            _messagesProcessor.AddHandler(new InputMessageHandler(b => IsInputVisible = b));

            _messagesProcessor.AddHandler(new InputMessageHandler(b => IsInputVisible = b));
            _messagesProcessor.AddHandler(new WebSessionConnectionHandler(loggerFactory, isConnected => IsBusy = !isConnected));

            HandleMessageCommand = ReactiveCommand.CreateFromTask<Message>(HandleMessageAsync);
            //SendCommand = ReactiveCommand.CreateFromTask(SendAsync);

            _userAccountContainer = userAccountContainer;

            IsBusy = true;

            RapidProMessageAppearingCommand = new Command<RapidProMessage>(OnRapidProMessageAppearing);
            RapidProMessageDisappearingCommand = new Command<RapidProMessage>(OnRapidProMessageDisappearing);

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(InputText))
                {
                    SendMessageAsync(InputText);
                    InputText = string.Empty;
                }

            });

            OnActionSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(ActionInputText))
                {
                    SendMessageAsync(ActionInputText);
                    ActionInputText = string.Empty;
                }

                if (!string.IsNullOrEmpty(RapidProMessageId))
                {
                    RemoveChatRapidProMessageQuickReplie(RapidProMessageId);
                }
            });

            NavigateBackCommand = ReactiveCommand.CreateFromTask(NavigateBackAsync);

            _rapidProContainer = new RapidProContainer();
            _rapidProService = new RapidProService();

            _firebaseContainer = new FirebaseContainer();

            _chatDatabase = new ChatDatabase();
        }

        //public string Uri { get; }

        public bool IsInputVisible { get; private set; } = true;

        public Interaction<Message, bool> SendMessageInteraction { get; }

        public IInfoViewModel InfoViewModel { get; private set; }

        //public ICommand SendCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand HandleMessageCommand { get; }

        public IUriOpeningHandler UriOpeningHandler { get; }

        public async void FcmAndRapidProInit()
        {
            try
            {
                if (string.IsNullOrEmpty(_firebaseContainer.FirebaseChannelHost) && string.IsNullOrEmpty(_firebaseContainer.FirebaseChannelId))
                {
                    var title = Resources.Localization.Chat_Dialog_TitleText;
                    var description = Resources.Localization.Chat_Dialog_DescriptionText;

                    var result = await _notificationManager.ShowNotificationAsync(
                            title,
                            description,
                            Resources.Localization.Chat_Dialog_AcceptBtn);

                    if (result)
                    {
                        IsBusy = false;
                        await _changeCountryProgramRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
                    }
                }
                else
                {
                    InitializeFcmAndRapidPro();
                    //OnInitialized();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - FcmAndRapidProInit: Exception - {ex.Message.ToString()}");
            }
        }

        public async void ApiReadyMessage()
        {
            //await _messageInteractor.SendInit(InitPhrase);
            //OnInitialized();
        }

        public void SetErrorState()
        {
            InfoViewModel = InfoViewModelFactory.CreateViewModel(ErrorImgSource, Covi.Resources.Localization.Exception_NoInternetConnection);
            IsBusy = false;
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(ChatViewModel));
        }

        private void OnInitialized()
        {
            IsBusy = false;
        }

        public void HideBusy()
        {
            IsBusy = false;
        }

        public void ShowBusy()
        {
            IsBusy = true;
        }

        //private async Task SendAsync()
        //{
        //    //await _messageInteractor.SendText(InputText);
        //    InputText = string.Empty;
        //}

        private async Task NavigateBackAsync()
        {
            await _navigationServiceDelegate.GoBackAsync();
        }

        private async Task HandleMessageAsync(Message message, CancellationToken ct = default)
        {
            await _messagesProcessor.HandleAsync(message);
        }

        void OnRapidProMessageAppearing(RapidProMessage rapidProMessage)
        {
            //var idx = RapidProMessages.IndexOf(rapidProMessage);
            //if (idx <= 8)
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        //while (DelayedMessages.Count > 0)
            //        //{
            //        //    Messages.Insert(0, DelayedMessages.Dequeue());
            //        //}
            //        ShowScrollTap = false;
            //        //LastMessageVisible = true;
            //        //PendingMessageCount = 0;
            //    });
            //}
        }

        void OnRapidProMessageDisappearing(RapidProMessage rapidProMessage)
        {
            //var idx = RapidProMessages.IndexOf(rapidProMessage);
            //if (idx >= 8)
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        ShowScrollTap = true;
            //        //LastMessageVisible = false;
            //    });
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void InsertRapidProMessages(RapidProFcmPushNotification rapidProFcmPushNotification)
        {
            var rapidProMessage = new RapidProMessage() { PId = Guid.NewGuid().ToString(), Text = rapidProFcmPushNotification.Body, Value = rapidProFcmPushNotification.Body, User = MessageUserEnum.UserHealthBuddy.ToDescriptionAttr(), MessageAction = false, ChannelId = _firebaseContainer.FirebaseChannelId, CreatedDateTime = DateTime.UtcNow };
            InsertChatRapidProMessage(rapidProMessage);

            if (rapidProFcmPushNotification.QuickReplies != null)
            {
                var quickReplies = JsonConvert.DeserializeObject<List<string>>(rapidProFcmPushNotification.QuickReplies);
                if (quickReplies != null)
                {
                    string rapidProMessageId = Guid.NewGuid().ToString();
                    foreach (var quickReplie in quickReplies)
                    {
                        var rapidProMessageQuickReplie = new RapidProMessage() { PId = Guid.NewGuid().ToString(), Id = rapidProMessageId, Text = quickReplie, Value = quickReplie, User = MessageUserEnum.UserHealthBuddy.ToDescriptionAttr(), MessageAction = true, ChannelId = _firebaseContainer.FirebaseChannelId, CreatedDateTime = DateTime.UtcNow };
                        InsertChatRapidProMessage(rapidProMessageQuickReplie);
                    }
                }
            }

            this.HideBusy();
        }

        private async void GetChatRapidProMessages()
        {
            try
            {
                var rapidProMessages = await _chatDatabase.GetRapidProMessagesByChannelIdAsync(_firebaseContainer.FirebaseChannelId);
                if (rapidProMessages.Any())
                {
                    foreach (var rapidProMessage in rapidProMessages)
                    {
                        RapidProMessages.Insert(0, rapidProMessage);
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - GetChatRapidProMessages: Exception - {ex.Message.ToString()}");
            }
        }

        private async void InsertChatRapidProMessage(RapidProMessage rapidProMessage)
        {
            try
            {
                if (rapidProMessage != null)
                {
                    RapidProMessages.Insert(0, rapidProMessage);
                    await _chatDatabase.InsertRapidProMessageAsync(rapidProMessage);
                    if (_rapidProContainer.RapidProIsChatDatabase == false)
                    {
                        _rapidProContainer.RapidProIsChatDatabase = true;
                        Console.WriteLine($"ChatViewModel - InsertChatRapidProMessage: RapidProChatDatabase");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - InsertChatMessage: Exception - {ex.Message.ToString()}");
            }
        }

        private async void RemoveChatRapidProMessageQuickReplie(string rapidProMessageId)
        {
            try
            {
                var rapidProMessageQuickReplies = RapidProMessages.Where(x => x.Id == rapidProMessageId).ToList();
                foreach (var rapidProMessageQuickReplie in rapidProMessageQuickReplies)
                {
                    RapidProMessages.Remove(rapidProMessageQuickReplie);
                    await _chatDatabase.DeleteRapidProMessageQuickReplieAsync(rapidProMessageQuickReplie);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - RemoveChatRapidProMessage: Exception - {ex.Message.ToString()}");
            }
        }


        #region RapidPro Fcm Push Notification

        private async void InitializeFcmAndRapidPro()
        {
            this.ShowBusy();

            try
            {
                _userAccountInfo = await _userAccountContainer.GetAsync().ConfigureAwait(false);

                if (_rapidProContainer.RapidProIsChatDatabase == true && _rapidProContainer.RapidProIsInitMsg == true)
                {
                    GetChatRapidProMessages();

                    Console.WriteLine($"ChatViewModel - InitializeFcmAndRapidPro: ChatDatabase");
                }
                else
                {
                    if (string.IsNullOrEmpty(_rapidProContainer.RapidProFcmToken))
                    {
                        string fcmPushNotificationToken = CrossFirebasePushNotification.Current?.Token;
                        _rapidProContainer.RapidProFcmToken = fcmPushNotificationToken;

                        Console.WriteLine($"ChatViewModel - InitializeFcmAndRapidPro: Token - {fcmPushNotificationToken}");
                    }

                    if (string.IsNullOrEmpty(_rapidProContainer.RapidProUrn))
                    {
                        string rapidProUrn = RapidProHelper.GetUrnFromGuid();
                        _rapidProContainer.RapidProUrn = rapidProUrn;

                        Console.WriteLine($"ChatViewModel - InitializeFcmAndRapidPro: Urn - {rapidProUrn}");
                    }

                    await RapidProRegisterAndReceiveInit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - InitializeFcmAndRapidPro: Exception - {ex.Message.ToString()}");
            }
        }

        private async Task RapidProRegisterAndReceiveInit()
        {
            try
            {
                string rapidProUrn = _rapidProContainer.RapidProUrn;
                string rapidProFcmToken = _rapidProContainer.RapidProFcmToken;

                if (_rapidProContainer.RapidProIsInit == false)
                {
                    if (!string.IsNullOrEmpty(rapidProUrn) && !string.IsNullOrEmpty(rapidProFcmToken))
                    {
                        var rapidProRegister = await _rapidProService.RapidProRegister(rapidProUrn, rapidProFcmToken);
                        if (!string.IsNullOrEmpty(rapidProRegister.ContactUuid))
                        {
                            var rapidProReceive = _rapidProService.RapidProReceive(rapidProUrn, rapidProFcmToken, RapidProInitPhrase);
                            _rapidProContainer.RapidProIsInit = true;
                            _rapidProContainer.RapidProIsInitMsg = true;
                        }
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - RapidProRegisterAndReceiveInit: Exception - {ex.Message.ToString()}");
            }
        }

        private void SendMessageAsync(string inputText)
        {
            try
            {
                this.ShowBusy();

                string rapidProUrn = _rapidProContainer.RapidProUrn;
                string rapidProFcmToken = _rapidProContainer.RapidProFcmToken;

                if (!string.IsNullOrEmpty(rapidProUrn) && !string.IsNullOrEmpty(rapidProFcmToken))
                {
                    //if (_rapidProContainer.RapidProIsInit == true && _rapidProContainer.RapidProIsInitMsg == true)
                    if (_rapidProContainer.RapidProIsInit == true)
                    {
                        string user = _userAccountInfo.IsAnonymous ? MessageUserEnum.UserAnonymous.ToDescriptionAttr() : MessageUserEnum.UserLogin.ToDescriptionAttr();
                        string userName = _userAccountInfo.UserAccount != null ? _userAccountInfo.UserAccount.Username : string.Empty;
                        var rapidProMessage = new RapidProMessage() { PId = Guid.NewGuid().ToString(), Text = inputText, Value = inputText, User = user, UserName = userName, ChannelId = _firebaseContainer.FirebaseChannelId, CreatedDateTime = DateTime.UtcNow };
                        InsertChatRapidProMessage(rapidProMessage);

                        var rapidProReceive = _rapidProService.RapidProReceive(rapidProUrn, rapidProFcmToken, inputText);
                        _rapidProContainer.RapidProIsInitSend = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatViewModel - SendMessageAsync: Exception - {ex.Message.ToString()}");
            }
        }

        #endregion
    }
}
