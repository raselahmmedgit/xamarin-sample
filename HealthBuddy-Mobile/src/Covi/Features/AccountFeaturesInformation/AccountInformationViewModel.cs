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

using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Covi.Features.AccountFeaturesInformation.Flows;
using Covi.Features.AccountFeaturesInformation.Routes;
using Covi.Features.AccountFeaturesInformation.Services;
using Covi.Features.Analytics;
using Covi.Utils;
using Prism.Navigation;
using ReactiveUI;

namespace Covi.Features.AccountFeaturesInformation
{
    public class AccountInformationViewModel : ComponentViewModelBase
    {
        private IAccountFeaturesInformationHandler _currentAccountFeaturesInformationHandler;
        private readonly IAccountInformationService _accountInformationService;

        public ReactiveCommand<Unit, Unit> GoToNextStepCommand { get; }

        public AccountInformationViewModel(IAccountInformationService accountInformationService)
        {
            _accountInformationService = accountInformationService;
            GoToNextStepCommand = ReactiveCommand.CreateFromTask(HandleNextStepAsync);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitializeAsync(parameters).FireAndForget();
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(AccountInformationViewModel));
        }

        private async Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(AccountInformationRoute.AccountInfoTypeName, out string parameter))
            {
                _currentAccountFeaturesInformationHandler = _accountInformationService.GetCurrentAccountInfoType(parameter);
                InformationItemsList = await _currentAccountFeaturesInformationHandler.GetInfoItemsAsync();
            }
        }

        public IReadOnlyList<InformationItem> InformationItemsList { get; set; }

        private async Task HandleNextStepAsync()
        {
            try
            {
                IsBusy = true;
                await _currentAccountFeaturesInformationHandler.HandleNextPageCommandAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
