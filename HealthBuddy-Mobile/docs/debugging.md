# Debugging

For the sake of debug simplicity you can use predefined `DeveloperToolsViewModel`. To navigate to it you should register `DeveloperToolsPageProviderService` in the `MainFeature` registering process:

```cs
this.RegisterFeature<MainFeature>((provider, feature) =>
    feature.RegisterPageService<NewsfeedPageProviderService>(provider)
        .RegisterPageService<PollsPageProviderService>(provider)
        .RegisterPageService<ChatTabbedPageProviderService>(provider)
        .RegisterPageService<RumoursTabbedPageProviderService>(provider)
        .RegisterPageService<MenuPageProviderService>(provider)
        // The following line will add another tab to the main application page. 
        .RegisterPageService<DeveloperToolsPageProviderService>(provider));
```

You can customize this viewmodel depending on the features you want to debug in runtime.

Also you should add debugging module as the part of the application initialization process in a project specific prism platform initializer:

```cs
    public void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<Covi.Features.Debugging.DebuggingModule>(InitializationMode.WhenAvailable);
    }
```

In the platform specific entry points search for the following lines
```cs
#if DEBUG || ANALYTICS
    if (!string.IsNullOrWhiteSpace(Constants.AppCenterConstants.Secret_iOS))
    {
        Microsoft.AppCenter.AppCenter.Start(Constants.AppCenterConstants.Secret_iOS,
            typeof(Microsoft.AppCenter.Analytics.Analytics),
            typeof(Microsoft.AppCenter.Crashes.Crashes));
            Covi.Logs.Logger.Factory.AddProvider(new Covi.iOS.Services.Log.AppCenterLogProvider());
    }
#endif
```
These conditional flags enables crash reporting and analytics (if AppCenter key is provided), but also 
logging to console and AppCenter (throe analytics feature)  (logs are very extensive and protocol everything in the app, keep in mind when on metered connection)
``` cs
Covi.Logs.Logger.Factory.AddProvider(new Covi.iOS.Services.Log.AppCenterLogProvider());
```
