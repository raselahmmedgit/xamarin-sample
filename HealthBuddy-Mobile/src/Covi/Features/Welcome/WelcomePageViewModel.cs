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
using Prism.Navigation;
using Covi.Features.AccountFeaturesInformation;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;
using Covi.Features.CreateProfile;
using System.Reactive.Disposables;
using Covi.Features.Analytics;
using Covi.Utils;

namespace Covi.Features.Welcome
{
    public class WelcomePageViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly INavigationService _navigationService;

        public ReactiveCommand<Unit, Unit> GoToCreateProfile { get; }
        public ReactiveCommand<Unit, Unit> LogInCommand { get; }
        public ReactiveCommand<Unit, Unit> GoAnonymouslyCommand { get; }

        public WelcomePageViewModel(
            IMediator mediator,
            INavigationService navigationService)
        {
            _mediator = mediator;
            _navigationService = navigationService;

            GoToCreateProfile = ReactiveCommand.CreateFromTask(HandleGoToCreateProfileAsync);
            LogInCommand = ReactiveCommand.CreateFromTask(HandleLogInAsync);
            GoAnonymouslyCommand = ReactiveCommand.CreateFromTask(HandleAnonymousAsync);
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(WelcomePageViewModel));
        }

        private async Task HandleLogInAsync()
        {
            await _mediator.Send(new NavigateToLogInAction());
        }

        private async Task HandleGoToCreateProfileAsync()
        {
            await _mediator.Send(new AccountFeaturesInformationAction("Registered"));
        }

        private async Task HandleAnonymousAsync()
        {
            await _mediator.Send(new AccountFeaturesInformationAction("Anonymous"));
        }
    }
}
