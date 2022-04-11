#pragma warning disable SA1636 // File header copyright text should match
// https://github.com/dansiegel/Prism.Container.Extensions/blob/87dc7cd7e938adf462037b88d0528691cbfb9f58/src/Shiny.Prism/Navigation/INavigationServiceDelegate.cs
#pragma warning restore SA1636 // File header copyright text should match

using Prism.Navigation;

namespace Covi.Services.Navigation
{
    /// <summary>
    /// Provides an ability to perform <c>absolute</c> navigation from a context not bound to any page.
    /// </summary>
    /// <remarks>
    /// See for details https://github.com/dansiegel/Prism.Container.Extensions/blob/0322222eb1d4d08b00e0024cf22900db9f284ebe/docs/shiny/navigation.md
    /// </remarks>
    public interface INavigationServiceDelegate : INavigationService, IPlatformNavigationService
    {

    }
}
