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

using System.Reactive;
using System.Threading.Tasks;
using Covi.Services;
using Covi.Services.Navigation;
using Prism.Navigation;
using ReactiveUI;

namespace Covi.Features.ForceUpdate
{
    public class ForceUpdateViewModel : ViewModelBase
    {
        private readonly IAppStoreService _appStoreService;
        public ReactiveCommand<Unit, Unit> ButtonCommand { get; private set; }

        public string Description { get; private set; }

        public ForceUpdateViewModel(IAppStoreService appStoreService)
        {
            _appStoreService = appStoreService;
            ButtonCommand = ReactiveCommand.CreateFromTask(HandleButtonCommandAsync);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitializeAsync(parameters).FireAndForget();
        }

        private async Task InitializeAsync(INavigationParameters parameters)
        {
            string message = null;
            if (parameters != null)
            {
                var errorMessageParameters = await parameters.GetNavigationParametersAsync<ErrorMessageParameters>();
                if (errorMessageParameters != null)
                {
                    message = errorMessageParameters.ErrorMessage;
                }
            }

            Description = message;

        }

        private async Task HandleButtonCommandAsync()
        {
            await _appStoreService.OpenAppPageInStoreAsync();
        }
    }
}
