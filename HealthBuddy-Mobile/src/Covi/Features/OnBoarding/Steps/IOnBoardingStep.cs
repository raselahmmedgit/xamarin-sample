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

namespace Covi.Features.OnBoarding.Steps
{
    /// <summary>
    /// This interface provides an opportunity to create a onBoardingStep for OnBoarding process.
    /// </summary>
    public interface IOnBoardingStep
    {
        /// <summary>
        /// Provides a task for some action needed before onBoardingStep can be finished.
        /// It could be some permission request for example.
        /// </summary>
        /// <returns>
        /// In case onBoardingStep is not mandatory - always return yes.
        /// In case you wouldn't allow user to process without some restriction - return false.
        /// </returns>
        Task<bool> HandleStepAsync();

        /// <summary>
        /// Gets a Title of OnBoarding Step.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets a SubTitle of OnBoarding Step.
        /// </summary>
        string SubTitle { get; }

        /// <summary>
        /// Gets a IconCode of OnBoarding Step.
        /// </summary>
        string IconCode { get; }

        /// <summary>
        /// Gets a Error Message of OnBoarding Step.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets a Instructions of OnBoarding Step.
        /// </summary>
        IReadOnlyList<InstructionItem> Instructions { get; }

        /// <summary>
        /// Returns whether this onboarding step should be processed.
        /// </summary>
        Task<bool> IsStepAvailableAsync();
    }
}
