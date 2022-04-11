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

using Covi.Features.Analytics;
using Covi.Features.AppSettings;
using Covi.Utils;
using Covi.Utils.ReactiveCommandHelpers;
using MediatR;
using Prism.Navigation;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Unit = System.Reactive.Unit;

namespace Covi.Features.OnBoarding
{
    public class OnBoardingViewModel : ComponentViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly IOnBoardingSetupService _onBoardingSetupService;
        private readonly IAppPreferencesService _appPreferencesService;

        public int SelectedActivityIndex { get; private set; }
        public IList<OnBoardingItemViewModel> OnBoardingActivities { get; private set; }
        public int ActivitiesCount { get; private set; }
        public ReactiveCommand<Unit, Unit> GoToNextStepCommand { get; }
        public bool HasManyActivities => OnBoardingActivities?.Count > 1;

        public OnBoardingViewModel(
            IMediator mediator,
            IOnBoardingSetupService onBoardingSetupService,
            IAppPreferencesService appPreferencesService)
        {
            IsBusy = true;
            GoToNextStepCommand = ReactiveCommandFactory.CreateLockedCommand(HandleNextStepAsync);

            _mediator = mediator;
            _onBoardingSetupService = onBoardingSetupService;
            _appPreferencesService = appPreferencesService;
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var steps = _onBoardingSetupService.GetOnBoardingSteps();

            var processedSteps = steps.ToList();

            foreach (var step in steps)
            {
                if (!await step.IsStepAvailableAsync())
                {
                    processedSteps.Remove(step);
                }
            }

            OnBoardingActivities = processedSteps
                .Select(s => new OnBoardingItemViewModel(s))
                .ToList();
            ActivitiesCount = OnBoardingActivities.Count;

            if (!OnBoardingActivities.Any())
            {
                await SetOnboardingCompletedAsync();
            }

            IsBusy = false;
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(OnBoardingViewModel));
        }

        private async Task HandleNextStepAsync()
        {
            if (SelectedActivityIndex < OnBoardingActivities.Count)
            {
                var result = await OnBoardingActivities[SelectedActivityIndex].HandleStepAsync();

                if (result)
                {
                    SelectedActivityIndex++;
                }
            }

            if (SelectedActivityIndex == OnBoardingActivities.Count)
            {
                await SetOnboardingCompletedAsync();
            }
        }

        private async Task SetOnboardingCompletedAsync()
        {
            _appPreferencesService.SetOnBoardingState(true);
            await _mediator.Send(new OnBoardingFinishedAction());
        }
    }
}
