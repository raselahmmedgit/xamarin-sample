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
using System.Threading.Tasks;

namespace Covi.Features.OnBoarding.Steps
{
    public abstract class OnBoardingStepBase : IOnBoardingStep
    {
        public string Title { get; protected set; }

        public string SubTitle { get; protected set; }

        public string IconCode { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public IReadOnlyList<InstructionItem> Instructions { get; protected set; }

        public abstract Task<bool> HandleStepAsync();

        public virtual Task<bool> IsStepAvailableAsync()
        {
            return Task.FromResult(true);
        }
    }
}
