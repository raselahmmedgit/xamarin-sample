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

using Covi.Services.Dialogs;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Covi.Features.Components.DialogPage
{
    public class DialogViewModelBase : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;

        public DialogViewModelBase(
            ILoggerFactory loggerFactory,
            IDialogService dialogService)
        {
            _logger = loggerFactory.CreateLogger(typeof(DialogViewModelBase));
            _dialogService = dialogService;

            CloseDialogCommand = ReactiveCommand.CreateFromTask<NavigationParameters>(CloseDialogAsync);

            OnDialogCloseInteraction = new Interaction<Unit, Unit>();
            OnDialogDataLoadedInteraction = new Interaction<Unit, Unit>();
        }

        public Interaction<Unit, Unit> OnDialogCloseInteraction { get; }
        public Interaction<Unit, Unit> OnDialogDataLoadedInteraction { get; }
        public ReactiveCommand<NavigationParameters, Unit> CloseDialogCommand { get; }

        public virtual void OnDialogDataLoaded(INavigationParameters parameters)
        {
        }

        protected async Task CloseDialogAsync(NavigationParameters navigationParameters = null)
        {
            try
            {
                await OnDialogCloseInteraction.Handle(Unit.Default);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to interact with dialog view. Cannot handle OnDialogClose interaction.");
            }

            await _dialogService.CloseDialogAsync(navigationParameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            OnDialogDataLoaded(parameters);

            try
            {
                await OnDialogDataLoadedInteraction.Handle(Unit.Default);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to interact with dialog view. Cannot handle OnDialogDataLoaded interaction.");
            }
        }
    }
}
