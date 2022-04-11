# Component service continuous loading

## About
**Continuous loading** - technique that displays information for the user in small chunks, as user continues to scroll down the content. New chunks of information will be loaded in advance when user nears the end of the current content, thus creating an impression of the infinite list of information. This technique not only gives better user experience but decreases the network payload.

## Implementation
To add continuous loading to the component service you should follow these steps:

1. Your specific implementation of the component `IComponentService` should implement [`IInfiniteLoadable`](../../../src/Covi/Features/!Base/ComponentsManagement/IInfiniteLoadable.cs) interface;
2. Add [`InfiniteScrollBehavior`](../../../src/Covi/Features/!Base/ComponentsManagement/InfiniteScrollBehavior.cs) to the `ComponentHost` visual control which will host the region with your component service:

```xml
<ContentPage
...
    xmlns:components="clr-namespace:Covi.Features.ComponentsManagement" />

    <components:ComponentsHost
        RegionManager="{Binding HomeRegion}">
        <components:ComponentsHost.Behaviors>
            <components:InfiniteScrollBehavior/>
        </components:ComponentsHost.Behaviors>
    </components:ComponentsHost>
```

These steps will enable the following behaviour: as soon as user nears the end of the content provided by your component service (not necessary the end of the whole region manager content), `IInfiniteLoadable.LoadMoreIfNeeded()` will be called. Remember that this method will be called regardless of the state of the new information chunk loading, so it may be called multiple times.

That means that the actual behavior of the component service loading should be implemented in a different way for each specific component service.