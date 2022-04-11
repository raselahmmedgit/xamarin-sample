#pragma warning disable SA1636 // File header copyright text should match
// https://github.com/dansiegel/Prism.Container.Extensions/blob/87dc7cd7e938adf462037b88d0528691cbfb9f58/src/Shiny.Prism/Navigation/NavigationExtensions.cs
#pragma warning restore SA1636 // File header copyright text should match

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Prism.Common;

namespace Covi.Services.Navigation
{
    public static class NavigationExtensions
    {
        public static void UseNavigationDelegate(this IServiceCollection services)
        {
            services.AddSingleton<INavigationServiceDelegate, NavigationServiceDelegate>();
            if (!services.Any(x => x.ServiceType == typeof(IApplicationProvider)))
            {
                services.AddSingleton<IApplicationProvider, ApplicationProvider>();
            }
        }
    }
}
