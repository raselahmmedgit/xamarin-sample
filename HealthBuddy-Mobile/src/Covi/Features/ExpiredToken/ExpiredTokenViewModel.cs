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
using Covi.Features.Account.Services.SignOut;
using Covi.Features.Chat.Data;
using Covi.Features.CreateProfile;
using Covi.Services.Navigation;
using MediatR;
using Prism.Navigation;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Covi.Features.ExpiredToken
{
    public class ExpiredTokenViewModel : ViewModelBase
    {
        private readonly ISignOutService _signOutService;
        private readonly IMediator _mediator;
        private readonly ChatDatabase _chatDatabase;

        public string Description { get; private set; }

        public ReactiveCommand<Unit, Unit> ButtonCommand { get; set; }

        public ExpiredTokenViewModel(ISignOutService signOutService, IMediator mediator)
        {
            _signOutService = signOutService;
            _mediator = mediator;
            _chatDatabase = new ChatDatabase();

            ButtonCommand = ReactiveCommand.CreateFromTask(HandleButtonCommandAsync);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitializeAsync(parameters).FireAndForget();
        }

        private async Task InitializeAsync(INavigationParameters parameters)
        {
            await _signOutService.SignOutAsync();
            await _chatDatabase.DeleteAllRapidProMessageAsync();

            string message = null;
            if (parameters != null)
            {
                var errorMessageParameters = await parameters?.GetNavigationParametersAsync<ErrorMessageParameters>();
                if (errorMessageParameters != null)
                {
                    message = errorMessageParameters.ErrorMessage;
                }
            }

            Description = message;
        }

        private async Task HandleButtonCommandAsync()
        {
            await _mediator.Send(new NavigateToLogInAction());
        }
    }
}
