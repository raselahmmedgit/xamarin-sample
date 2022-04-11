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

using Prism.Navigation;
using System.Threading.Tasks;

namespace Covi.Services.Dialogs
{
    public interface IDialogService
    {
        Task<INavigationResult> ShowDialogAsync<TRequest>(string dialogName, TRequest request)
            where TRequest : DialogParametersBase;

        Task<Prism.Services.Dialogs.IDialogResult> ShowPrismDialogAsync<TRequest>(string dialogName, TRequest request)
            where TRequest : DialogParametersBase;

        Task CloseDialogAsync<TResult>(TResult resultParams)
            where TResult : INavigationParameters;

        Task CloseDialogAsync();
    }
}
