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
using System.Threading.Tasks;
using MediatR;

namespace Covi.Features.AccountFeaturesInformation.Flows
{
    public class RegisteredAccountFeaturesInformationHandler : IAccountFeaturesInformationHandler
    {
        private readonly IMediator _mediator;
        private readonly IReadOnlyList<InformationItem> _informationItems;

        public RegisteredAccountFeaturesInformationHandler(IMediator mediator)
        {
            _mediator = mediator;
            _informationItems = new List<InformationItem>
            {
                new InformationItem(Resources.Localization.RegisteredProfile_Instruction_Text1, "resource://Covi.Features.AccountFeaturesInformation.Resources.Images.locked.svg"),
                new InformationItem(Resources.Localization.RegisteredProfile_Instruction_Text2, "resource://Covi.Features.AccountFeaturesInformation.Resources.Images.health_wellbeing.svg"),
                new InformationItem(Resources.Localization.RegisteredProfile_Instruction_Text3, "resource://Covi.Features.AccountFeaturesInformation.Resources.Images.highlight.svg")
            };
        }

        public async Task HandleNextPageCommandAsync()
        {
            await _mediator.Send(new CreateProfileAction());
        }

        public Task<IReadOnlyList<InformationItem>> GetInfoItemsAsync()
        {
            return Task.FromResult(_informationItems);
        }
    }
}
