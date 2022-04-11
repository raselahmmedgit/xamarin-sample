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
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Covi.Services.Storage;

namespace Covi.Features.UserProfile.Services
{
    public class UserAccountContainer : IUserAccountContainer
    {
        private const string StorageKeyName = "UserProfile";
        private readonly IStorageService _storageService;
        private readonly Subject<UserAccountInfo> _subject = new Subject<UserAccountInfo>();

        public IObservable<UserAccountInfo> Changes { get; }

        public UserAccountContainer(IStorageService storageService)
        {
            _storageService = storageService;
            Changes = Observable.FromAsync(GetAsync)
                .Catch<UserAccountInfo, Exception>(ex =>
                {
                    System.Diagnostics.Debug.WriteLine($"Get UserAccount error, return default value.");
                    return Observable.Return<UserAccountInfo>(null);
                })
                .Concat(_subject)
                .DistinctUntilChanged()
                .Synchronize()
                .Replay(1)
                .RefCount();
        }

        public async Task<UserAccountInfo> GetAsync()
        {
            return await _storageService.GetAsync<UserAccountInfo>(StorageKeyName).ConfigureAwait(false);
        }

        public async Task SetAsync(UserAccountInfo data)
        {
            await _storageService.AddAsync(StorageKeyName, data).ConfigureAwait(false);
            _subject.OnNext(data);
        }
    }
}
