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

using Covi.Features.OnBoarding.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covi.Features.OnBoarding.Steps
{
    public class MadeForYouOnBoardingStep : OnBoardingStepBase, IOnBoardingStep
    {
        private const string TailoredForYouImage = "resource://Covi.Features.OnBoarding.Resources.Images.tailored_for_you.svg";

        public MadeForYouOnBoardingStep()
        {
            Title = Localization.OnBoarding_MadeForYou_Title.ToUpper();
            SubTitle = Localization.OnBoardind_MadeForYou_SubTitle;
            IconCode = TailoredForYouImage;
            Instructions = new List<InstructionItem>
            {
                new InstructionItem(Localization.OnBoarding_MadeForYou_Instruction1),
                new InstructionItem(Localization.OnBoarding_MadeForYou_Instruction2),
                new InstructionItem(Localization.OnBoarding_MadeForYou_Instruction3)
            };
        }

        public override Task<bool> HandleStepAsync()
        {
            return Task.FromResult(true);
        }
    }
}
