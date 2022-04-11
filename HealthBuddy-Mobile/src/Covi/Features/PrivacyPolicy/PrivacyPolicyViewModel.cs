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
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Covi.Features.Analytics;
using Covi.Features.Components.InfoView;
using Covi.Features.PrivacyPolicy.Services;
using Covi.Services.ErrorHandlers;
using Covi.Services.Http.ApiExceptions;
using Covi.Utils;
using Prism.Navigation;

namespace Covi.Features.PrivacyPolicy
{
    class PrivacyPolicyViewModel : ViewModelBase
    {
        private const string ErrorImgSource = "connection_problem.svg";
        private readonly IPrivacyPolicyService _privacyPolicyService;
        private readonly IErrorHandler _errorHandler;

        public string Title { get; private set; }
        public string LogoImageSource { get; private set; }
        public string PrivacyPolicyText { get; private set; }
        public bool HasError { get; private set; }

        public IInfoViewModel InfoViewModel { get; private set; }

        public PrivacyPolicyViewModel(IPrivacyPolicyService privacyPolicyService, IErrorHandler errorHandler)
        {
            _privacyPolicyService = privacyPolicyService;
            _errorHandler = errorHandler;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InfoViewModel = InfoViewModelFactory.CreateViewModel(
                ErrorImgSource, string.Empty, async () => { await InitializeAsync(); });
            InitializeAsync().FireAndForget();
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            AnalyticsProvider.Instance.LogViewModel(nameof(PrivacyPolicyViewModel));
        }

        private async Task InitializeAsync()
        {
            try
            {
                HasError = false;
                IsBusy = true;
                var privacyPolicyEntity = await _privacyPolicyService.GetPrivacyPolicyAsync();
                PrivacyPolicyText = privacyPolicyEntity.Body?.Text;
                LogoImageSource = privacyPolicyEntity.ImageLink?.Url;
                Title = privacyPolicyEntity.Title;
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
    }
}
