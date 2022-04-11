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
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;
using Covi.Features.MedicalChangeStatus.Services;
using Covi.Features.MedicalCodeSharing.Routes;
using Covi.Services.ApplicationMetadata;
using Covi.Services.ErrorHandlers;
using MediatR;
using Prism.Navigation;
using ReactiveUI;

using Unit = System.Reactive.Unit;

namespace Covi.Features.MedicalChangeStatus
{
    public class MedicalChangeStatusViewModel : ComponentViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly IErrorHandler _errorHandler;
        private readonly IMetadataContainer _metadataContainer;
        private readonly IMedicalChangeStatusService _medicalChangeStatusService;

        public bool HasError { get; private set; }

        public string ErrorMessage { get; private set; }

        public IReadOnlyCollection<MedicalOptionItemViewModel> StatusesList { get; private set; } = new List<MedicalOptionItemViewModel>();

        public ReactiveCommand<Unit, Unit> InitStatusCommand { get; private set; }

        public DateTime StatusChangeDate { get; set; }

        public string StatusChangeDateDisplayValue => StatusChangeDate.ToShortDateString();

        public MedicalChangeStatusViewModel(
            IMediator mediator,
            IMedicalChangeStatusService medicalChangeStatusService,
            IErrorHandler errorHandler,
            IMetadataContainer metadataContainer)
        {
            _mediator = mediator;
            _medicalChangeStatusService = medicalChangeStatusService;
            _errorHandler = errorHandler;
            _metadataContainer = metadataContainer;

            InitStatusCommand = ReactiveCommand.CreateFromTask(HandleApplyStatusCommandAsync);
        }

        public override void OnActivated(CompositeDisposable lifecycleDisposable)
        {
            base.OnActivated(lifecycleDisposable);

            StatusChangeDate = DateTime.Now;
            SetStatusesList();
        }

        private async void SetStatusesList()
        {
            IsBusy = true;

            var metadata = await _metadataContainer.GetAsync();
            var statusesList = new List<MedicalOptionItemViewModel>();

            if (metadata != null)
            {
                foreach (var status in metadata.Metadata.Statuses.Values)
                {
                    var option = new MedicalOptionItemViewModel(status.Id.Value, status.Name, UpdateSource);

                    statusesList.Add(option);
                }
            }

            StatusesList = statusesList;

            IsBusy = false;
        }

        private async Task HandleApplyStatusCommandAsync()
        {
            Validate();
            if (!HasError)
            {
                try
                {
                    IsBusy = true;

                    var statusToApply = StatusesList.First(s => s.IsSelected);
                    await _medicalChangeStatusService.InitStatusChangeAsync(statusToApply.OptionId, StatusChangeDate);

                    var navParams = new MedicalCodeSharingParameters(statusToApply.OptionText, string.Empty);

                    await _mediator.Send(new MedicalCodeShareAction(navParams));
                }
                catch (Exception e)
                {
                    await _errorHandler.HandleAsync(e);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void Validate()
        {
            HasError = false;
            ErrorMessage = string.Empty;

            if (!StatusesList.Any(s => s.IsSelected))
            {
                ErrorMessage = Resources.Localization.Medical_FirstStepEmpty_Error;
                HasError = true;
                return;
            }
        }

        private void UpdateSource(MedicalOptionItemViewModel item)
        {
            foreach (var option in StatusesList)
            {
                if (item.OptionId != option.OptionId)
                {
                    option.IsSelected = false;
                }
            }
        }
    }
}
