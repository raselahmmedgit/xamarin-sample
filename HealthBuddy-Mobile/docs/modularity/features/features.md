# Features separation

## About

Main purpose of the feature infrastructure is to reduce coupling between different components. With usual approach tight coupling is present due to the strict navigation flow dependencies: navigation from one page to the other was performed either by explicit call of the `INavigationService` or by the injection of the `IRoute` specific implementations, such as `ICreateProfileRoute`. In both cases in order to change navigation flow for specific page it is required to change implementation of the component.

Feature approach uses [MediatR](https://github.com/jbogard/MediatR) package to reduce coupling between components. When component reaches the point when some navigation (or, in general, any other action) should be performed, `IMediator.Send(IRequest request)` method should be called with the specific `IRequest` implementation. Name of such implementation should not be related with other components, when possible (e.g. use `UserLogInFinishedAction` instead of `StartOnBoardingAction`; however action handler names can be meaningful in such ways).

## Infrastructure

**Feature** - piece of application functionality, usually represented on the UI side as the application page. Similar to the **PRISM module** but feature can aggregate several modules and have custom initializer. Basic implementation is represented by the [`FeatureBase`](../../../src/Covi/Features/!Base/Registry/FeatureBase.cs) class and the [`IFeature`](../../../src/Covi/Features/!Base/Registry/IFeature.cs) interface.

**Action (or request)** - object which represents some action that should be executed. It can have parameters and/or properties to pass additional information. Must implement `MediatR.IRequest` - in cases when only one handler should be used (request should be passed to the method `Mediator.Send(IRequest request)`); or `MediatR.INotification` - in cases when multiple handlers should be used (notification should be passed to the method `Mediator.Publish(INotification notification)`). [Read more about MediatR](https://github.com/jbogard/MediatR/wiki#basics).

**Action handler (request handler)** - handler of the specific `IRequest` implementation (or of the `INotification`) to perform a designated action.

## Samples

### Actions

To create a plain action it is enough to implement `IRequest` or `INotification` interface:

```cs
public class NavigateToLogInAction : IRequest
{
}
```

To specify the type of the return value, generic `IRequest<TResponse>` should be used.

### Action handlers

To create a handler of the specific action (`TRequest` or `TNotification`) you should derive from `AsyncRequestHandler<TRequest>` (or `NotificationHandler<TNotification>`) and implement interface `IRequestHandler<TRequest, TResponse>` (or `INotificationHandler<TNotification>`):

```cs
public class NavigateToLogInActionHandler : AsyncRequestHandler<NavigateToLogInAction>, IRequestHandler<NavigateToLogInAction, Unit>
{
    private readonly IUserLogInRoute _userLogInRoute;
    private readonly INavigationServiceDelegate _navigationServiceDelegate;

    public NavigateToLogInActionHandler(IUserLogInRoute userLogInRoute, INavigationServiceDelegate navigationServiceDelegate)
    {
        _userLogInRoute = userLogInRoute;
        _navigationServiceDelegate = navigationServiceDelegate;
    }

    protected override async Task Handle(NavigateToLogInAction request, CancellationToken cancellationToken)
    {
        await _userLogInRoute.ExecuteAsync(_navigationServiceDelegate).ConfigureAwait(false);
    }
}
```

### Trigger lists

Afterwards you should put created action handler into the list of triggers. Usually it is located inside the feature folder by the path `Actions/{FeatureName}Triggers.cs`. Trigger list is represented by static class which should contain all of the action handlers related to the feature. That means, if created handler executing navigation from one part of functionality to the other, **such handler should be registered in both trigger lists**. This will result better understanding of possible flows.

```cs
public static class CreateProfileTriggers
{
    public static class Navigation
    {
        public static class In
        {
            public static Type AppInitializedActionHandler { get; } = typeof(AppInitializedActionHandler);

            public static Type CreateProfileActionHandler { get; } = typeof(CreateProfileActionHandler);
        }

        public static class Out
        {
            public static Type NavigateToLogInActionHandler { get; } = typeof(NavigateToLogInActionHandler);

            public static Type NavigateToOnBoardingActionHandler { get; } = typeof(ProfileCreatedActionHandler);
        }
    }
}
```

In case when action handlers are supposed to navigate from one feature to another, they should be put in the separate `Navigation.In` and `Navigation.Out` static classes for better clarity.

### Feature

Action handlers are registered in the scope of the feature registration. To create feature you should derive from the `FeatureBase` class. You can put any relevant to the feature dependencies in the `Register` method.

```cs
public class CreateProfileFeature : FeatureBase, IFeature
{
    public override void Register(IContainerProvider registry, IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<Features.CreateProfile.CreateProfileModule>(InitializationMode.WhenAvailable);
    }
}
```

Also you can override `ConfigureDefault` method to create default configuration which, in turn, can be overriden by custom configuration in the moment of the feature registration.

### Feature registration

Features should be registered in the method `RegisterFeatures` of the `App.xaml.cs` file. They are registered by the usage of the extension method `RegisterFeature<TFeature>(...)`. It has optional parameter `Action<IContainerProvider, TFeature> featureSetup` which should be used to override default feature configuration.

```cs
this.RegisterFeature<BaseServicesFeature>();

this.RegisterFeature<CreateProfileFeature>((provider, feature) =>
    feature.RegisterNavigationTriggers(
        provider,
        CreateProfileTriggers.Navigation.In.AppInitializedActionHandler,
        CreateProfileTriggers.Navigation.Out.NavigateToLogInActionHandler,
        CreateProfileTriggers.Navigation.Out.NavigateToOnBoardingActionHandler));

this.RegisterFeature<AccountInformationFeature>((provider, feature) =>
    feature.RegisterNavigationTriggers(
        provider,
        AccountInformationTriggers.Navigation.In.NavigateToAccountInformationActionHandler,
        AccountInformationTriggers.Navigation.Out.NavigateCreateProfile,
        AccountInformationTriggers.Navigation.Out.NavigateToHomeAnonymous));
```

`RegisterGeneralPurposeTriggers` or `RegisterNavigationTriggers` extension method should be used to register required action handlers. For visual clarity only feature-related action handler types should be passed to this extension method; moreover, you should use corresponding triggers lists classes.

Feature registration process can be customized with other extension methods, as it can be seen in the main feature registration, `RegisterPageService` method is used:

```cs
this.RegisterFeature<MainFeature>((provider, feature) =>
    feature.RegisterPageService<NewsfeedPageProviderService>(provider)
        .RegisterPageService<PollsPageProviderService>(provider)
        .RegisterPageService<ChatTabbedPageProviderService>(provider)
        .RegisterPageService<RumoursTabbedPageProviderService>(provider)
        .RegisterPageService<MenuPageProviderService>(provider));
```

Remember that **not** each and every action handler should be registered. Essentially this process of registration is application configuration as well: depending on such configuration some navigation flows of the application could be altered.

### Evoke an action

To execute some action you should use `Mediator.Send(IRequest request)` or `Mediator.Publish(INotification notification)` method:

```cs
private readonly IMediator _mediator;
...


private async Task HandleLogInAsync()
{
    await _mediator.Send(new AccountFeaturesInformationAction("Registered"));
}

private async Task HandleGoToCreateProfileAsync()
{
    await _mediator.Send(new CreateProfileAction());
}
```
