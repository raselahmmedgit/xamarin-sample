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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Covi.Features.ComponentsManagement;
using Covi.Utils;
using DynamicData;
using DynamicData.Binding;

using Nito.AsyncEx;

using Prism.Ioc;
using ReactiveUI;
using Xamarin.Forms;

#nullable enable
namespace Covi.Features.Regions
{
    public class RegionManager : BindableObject
    {
        private static readonly Dictionary<string, List<Type>> _componentServiceRegistry = new Dictionary<string, List<Type>>();
        private static readonly Dictionary<string, IDataTemplateProvider> _dataTemplateProviderRegistry = new Dictionary<string, IDataTemplateProvider>();

        private static Func<IContainerProvider>? ServiceProvider { get; set; }

        private readonly SemaphoreSlim _collectionUpdateSemaphore = new SemaphoreSlim(1);
        private SourceList<IComponentsGroup> _componentItems;
        private IList<IComponentService> _componentServices;

        public static void RegisterFunctionalityToTheRegion<TComponentService>(string regionIdentifier, IDataTemplateProvider dataTemplateProvider)
            where TComponentService : IComponentService
        {
            Guard.NotEmpty(regionIdentifier, nameof(regionIdentifier));

            if (_componentServiceRegistry.TryGetValue(regionIdentifier, out var componentServices))
            {
                componentServices.Add(typeof(TComponentService));
            }
            else
            {
                componentServices = new List<Type>()
                {
                    typeof(TComponentService)
                };
                _componentServiceRegistry.Add(regionIdentifier, componentServices);
            }

            var templateKey = GenerateRegionComponentKey(regionIdentifier, typeof(TComponentService));
            _dataTemplateProviderRegistry.Add(templateKey, dataTemplateProvider);
        }

        public static void SetResolver(Func<IContainerProvider> serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static RegionManager CreateForIdentifier(string regionIdentifier, HostContext hostContext)
        {
            Guard.NotEmpty(regionIdentifier, nameof(regionIdentifier));
            Guard.NotNull(ServiceProvider, nameof(ServiceProvider));

            List<IComponentService>? componentServices = null;
            if (_componentServiceRegistry.TryGetValue(regionIdentifier, out var componentTypes))
            {
                componentServices = componentTypes
                    .Select(ServiceProvider().Resolve)
                    .Cast<IComponentService>()
                    .ToList();
            }

            return new RegionManager(regionIdentifier, componentServices ?? new List<IComponentService>(), hostContext);
        }

        private static string GenerateRegionComponentKey(string regionIdentifier, Type serviceType)
        {
            return GenerateRegionComponentKey(regionIdentifier, serviceType.Name);
        }

        private static string GenerateRegionComponentKey(string regionIdentifier, string serviceKey)
        {
            return string.Concat(regionIdentifier, "&", serviceKey);
        }

        public static bool TryGetDataTemplateProvider(string regionIdentifier, Type serviceType, out IDataTemplateProvider? dataTemplateProvider)
        {
            var key = GenerateRegionComponentKey(regionIdentifier, serviceType);
            return _dataTemplateProviderRegistry.TryGetValue(key, out dataTemplateProvider);
        }

        public static bool TryGetDataTemplateProvider(string regionIdentifier, string serviceKey, out IDataTemplateProvider? dataTemplateProvider)
        {
            var key = GenerateRegionComponentKey(regionIdentifier, serviceKey);
            return _dataTemplateProviderRegistry.TryGetValue(key, out dataTemplateProvider);
        }

        private RegionManager(string regionIdentifier, IList<IComponentService> componentServices, HostContext hostContext)
        {
            RegionIdentifier = regionIdentifier;
            _componentServices = componentServices;

            _componentItems = new SourceList<IComponentsGroup>();
            _componentItems.Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out var components)
                .Synchronize()
                .Subscribe();

            RegionItems = components;

            foreach (var service in componentServices)
            {
                service.SetHostContext(hostContext);

                service.Items.Subscribe(async (items) =>
                {
                    // Workaround from the error System.ArgumentException: An item with the same key has already been added. Key: Covi.Features.ComponentsManagement.Internal.ComponentsGroup.
                    // This error is intermittent and will cause application crash.
                    using (await _collectionUpdateSemaphore.LockAsync())
                    {
                        await Task.Delay(100);
                        _componentItems.Edit((list) =>
                        {
                            if (items == null)
                            {
                                return;
                            }

                            var existingGroup = list.FirstOrDefault(i => i.Key == items.Key);
                            if (existingGroup != null)
                            {
                                var index = list.IndexOf(existingGroup);
                                list[index] = items;
                            }
                            else
                            {
                                list.Add(items);
                            }
                        });
                    }
                });
            }
        }

        public string RegionIdentifier { get; }

        public ReadOnlyObservableCollection<IComponentsGroup> RegionItems { get; private set; }

        public IComponentService GetComponentServiceByKey(string componentServiceKey)
        {
            return _componentServices.FirstOrDefault(s => string.Equals(s.ComponentKey, componentServiceKey));
        }

        /*private async void OnComponentsChanged(IChangeSet<IComponentsGroup> changeSet)
        {
            // This is a custom workaround implementation for CollectionView control usage.
            // TODO: Revert to the reactive subscriptions when fixed.
            // Due to unknown Android synchronization issue '.ObserveOn(RxApp.MainThreadScheduler)'
            // is not sufficient to apply change sets in the main thread: Android application will crash.
            // https://github.com/xamarin/Xamarin.Forms/issues/9753
            // This is a workaround to apply change sets manually.
            /*using (await _collectionUpdateSemaphore.LockAsync())
            {
                // To keep the sequence of changes additional synchronization is required.
                var taskCompletionSource = new TaskCompletionSource<bool>();
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        foreach (var change in changeSet)
                        {
                            // Only Add and Replace events should occur.
                            if (change.Reason == ListChangeReason.Add)
                            {
                                RegionItems.Add(change.Item.Current);
                            }
                            else if (change.Reason == ListChangeReason.Replace)
                            {
                                // This workaround prevents application from crashes.
                                if (Device.RuntimePlatform == Device.Android)
                                {
                                    RegionItems.RemoveAt(change.Item.PreviousIndex);
                                    RegionItems.Insert(change.Item.PreviousIndex, change.Item.Current);
                                }

                                if (Device.RuntimePlatform == Device.iOS)
                                {
                                    RegionItems[change.Item.PreviousIndex] = change.Item.Current;
                                }
                            }
                        }
                    }
                    finally
                    {
                        taskCompletionSource.SetResult(true);
                    }
                });

                await taskCompletionSource.Task;
            }
        }*/
    }
}
