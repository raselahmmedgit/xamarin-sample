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
using System.Threading.Tasks;
using ReactiveUI;

namespace Covi.Features.Components.InfoView
{
    public static class InfoViewModelFactory
    {
        public static IInfoViewModel CreateViewModel(string imageSourceName, string informationText, Action retry = null)
        {
            var viewModel = new InfoViewModel();
            InitViewModel(viewModel, imageSourceName, informationText, retry);
            
            return viewModel;
        }

        public static IInfoViewModel CreateComponentViewModel(string imageSourceName, string informationText, Action retry = null)
        {
            var viewModel = new InfoComponentViewModel();
            InitViewModel(viewModel, imageSourceName, informationText, retry);

            return viewModel;
        }

        private static void InitViewModel(IInfoViewModel viewModel, string imageSourceName, string informationText, Action retry = null)
        {
            viewModel.ImageSourceName = imageSourceName;
            viewModel.InformationText = informationText;
            viewModel.IsRetryAvailable = retry != null;

            if (retry != null)
            {
                viewModel.RetryCommand = ReactiveCommand.CreateFromTask(() => Task.Run(retry));
            }
        }


    }
}
