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

using Covi.Features.OnBoarding;
using Covi.Features.OnBoarding.Steps;
using Covi.Features.PushNotifications.Services;
using Covi.Features.OnBoarding.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covi.Features.Account.Services.Authentication;

namespace Covi.Features.PushNotifications.OnBoardingStep
{
    public class PushNotificationOnBoardingStep : OnBoardingStepBase, IOnBoardingStep
    {
        private const string NotificationsImage = "resource://Covi.Features.PushNotifications.Resources.Images.important_notifications.svg";

        private const string Instruction1Image = "resource://Covi.Features.PushNotifications.Resources.Images.notification_instruction1.svg";
        private const string Instruction2Image = "resource://Covi.Features.PushNotifications.Resources.Images.notification_instruction2.svg";
        private const string Instruction3Image = "resource://Covi.Features.PushNotifications.Resources.Images.notification_instruction3.svg";

        private readonly IPushNotificationsService _pushNotificationsService;
        private readonly IAuthenticationInfoService _authenticationInfoService;

        public PushNotificationOnBoardingStep(
            IPushNotificationsService pushNotificationsService,
            IAuthenticationInfoService authenticationInfoService)
        {
            _pushNotificationsService = pushNotificationsService;
            _authenticationInfoService = authenticationInfoService;

            Title = Localization.OnBoarding_Notifications_Title;
            IconCode = NotificationsImage;
            ErrorMessage = Localization.OnBoarding_Notifications_ErrorText;
            Instructions = new List<InstructionItem>
            {
                new InstructionItem(Localization.OnBoarding_Notifications_Instruction1, Instruction1Image),
                new InstructionItem(Localization.OnBoarding_Notifications_Instruction2, Instruction2Image),
                new InstructionItem(Localization.OnBoarding_Notifications_Instruction3, Instruction3Image)
            };
        }

        public override async Task<bool> HandleStepAsync()
        {
            await _pushNotificationsService.InitializeAsync();
            return true;
        }

        public override Task<bool> IsStepAvailableAsync()
        {
            var result = !_authenticationInfoService.IsAnonymous();
            return Task.FromResult(result);
        }
    }
}
