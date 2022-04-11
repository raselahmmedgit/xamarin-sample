#pragma warning disable SA1636 // File header copyright text should match
// https://github.com/dansiegel/Prism.Container.Extensions/blob/87dc7cd7e938adf462037b88d0528691cbfb9f58/src/Shiny.Prism/Navigation/NavigationServiceDelegate.cs
#pragma warning restore SA1636 // File header copyright text should match

using System;
using System.Threading.Tasks;
using Prism;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation;

namespace Covi.Services.Navigation
{
    internal class NavigationServiceDelegate : INavigationServiceDelegate
    {
        private IContainerProvider Container { get; }
        private IApplicationProvider ApplicationProvider { get; }

        public NavigationServiceDelegate(IContainerExtension container, IApplicationProvider applicationProvider)
        {
            Container = container;
            ApplicationProvider = applicationProvider;
        }

        #region INavigationService
        Task<INavigationResult> INavigationService.GoBackAsync()
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync();
        }

        Task<INavigationResult> INavigationService.GoBackAsync(INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync(parameters);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(Uri uri, INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri, parameters);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(string name)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name);
        }

        Task<INavigationResult> INavigationService.NavigateAsync(string name, INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name, parameters);
        }

        #endregion

        #region IPlatformNavigationService
        Task<INavigationResult> IPlatformNavigationService.GoBackAsync(INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackAsync(parameters, useModalNavigation, animated);
        }

        Task<INavigationResult> IPlatformNavigationService.GoBackToRootAsync(INavigationParameters parameters)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.GoBackToRootAsync(parameters);
        }

        Task<INavigationResult> IPlatformNavigationService.NavigateAsync(string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(name, parameters, useModalNavigation, animated);
        }

        Task<INavigationResult> IPlatformNavigationService.NavigateAsync(Uri uri, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            var navService = GetNavigationService();
            if (navService is null)
            {
                return PrismNotInitialized();
            }
            return navService.NavigateAsync(uri, parameters, useModalNavigation, animated);
        }
        #endregion

        private INavigationService GetNavigationService()
        {
            if (PrismApplicationBase.Current is null) return null;

            var navService = Container.Resolve<INavigationService>(PrismApplicationBase.NavigationServiceName);

            if (navService is IPageAware pa)
            {
                var mainPage = ApplicationProvider.MainPage;
                if (mainPage != null)
                {
                    pa.Page = PageUtilities.GetCurrentPage(mainPage);
                }
            }
            return navService;
        }

        private Task<INavigationResult> PrismNotInitialized()
        {
            INavigationResult result = new NavigationResult
            {
                Success = false,
                Exception = new NavigationException("No Prism Application Exists", null)
            };
            return Task.FromResult(result);
        }
    }
}
