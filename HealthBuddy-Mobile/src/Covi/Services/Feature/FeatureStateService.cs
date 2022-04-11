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
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Covi.Client.Services.Platform.Models;
using Covi.Services.ApplicationMetadata;

namespace Covi.Services.Feature
{
    public class FeatureStateService : IFeatureStateService
    {
        private readonly IMetadataContainer _metadataContainer;
        private TaskCompletionSource<bool> _metadataInitializationTaskCompletionSource;

        public FeatureStateService(IMetadataContainer metadataContainer)
        {
            _metadataContainer = metadataContainer;
            _metadataInitializationTaskCompletionSource = new TaskCompletionSource<bool>();
            Initialize();
            FeatureConfigurationChanged = _metadataContainer.Changes.Select(_ => Unit.Default);
            _metadataContainer.Changes.Select(m => m?.Metadata).Subscribe(OnMetadataChanged);
        }

        public IObservable<Unit> FeatureConfigurationChanged { get; }

        private Metadata Metadata { get; set; }

        private async void Initialize()
        {
            var metadataModel = await _metadataContainer.GetAsync();
            Metadata = metadataModel.Metadata;
            UpdateInitializationState();
        }

        private void OnMetadataChanged(Metadata value)
        {
            Metadata = value;
            UpdateInitializationState();
        }

        private void UpdateInitializationState()
        {
            if (!_metadataInitializationTaskCompletionSource.Task.IsCompleted
                && Metadata != null)
            {
                _metadataInitializationTaskCompletionSource.TrySetResult(true);
            }
        }

        public async Task<string> GetValueAsync(string key, string defaultValue)
        {
            await _metadataInitializationTaskCompletionSource.Task;
            return Metadata?.Features != null && Metadata.Features.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
