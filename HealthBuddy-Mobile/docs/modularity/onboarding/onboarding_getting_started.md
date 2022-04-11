# Onboarding process
## What is onboarding?
During first startup of the application user does not know anything about the app: how to use it, which system permission it wants to use and why. The application may have several enabled features which require specific system permissions, like push notification access. The onboarding process helps to solve it. This process is divided into several steps. Any step can be marked as 'mandatory' and user will not be able to go through onboarding process till permission accessed, in this case user will not be able to use the application until onboarding with required steps being completed.

## How to get started
When you decide which exactly feature do you need to be included in the app - you can choose them from default list of steps or write your own.

In order to include any default steps just go to **Covi/App.xaml.cs** file in method `ConfigureOnBoarding` and add appropriate lines there:

```cs
moduleCatalog.AddModule<Features.OnBoarding.Modules.MadeForYouModule>(InitializationMode.WhenAvailable);

moduleCatalog.AddModule<Features.OnBoarding.Modules.PrivacyModule>(InitializationMode.WhenAvailable);

moduleCatalog.AddModule<Features.BluetoothTracing.BluetoothTracingStep.BluetoothTracingModule>(InitializationMode.WhenAvailable);

moduleCatalog.AddModule<Features.PushNotifications.OnBoardingStep.PushNotificationOnBoardingModule>(InitializationMode.WhenAvailable);
```

Be aware that steps appear in the order they are defined.

If any module has a dependency to any other module - it should be added in method `ConfigureModuleCatalog`. 

For example `PushNotificationOnBoardingModule` has dependency to `PushNotificationsModule`, so you need to add next line in `ConfigureModuleCatalog`. 
```cs
moduleCatalog.AddModule<Features.PushNotifications.PushNotificationsModule>(InitializationMode.WhenAvailable);
```

### Sample steps

The application may include several steps you can use:

* Made for you - this step explains the main features of the application provides.
* Privacy - this step explains about the privacy that app provides him (anonymity, data ecryption, etc).
* Notifications - this step shows benefits of push notifications and offers user to opt in. Step is not 'mandatory' by default.

### Custom steps
[Create you own step](docs/modularity/onboarding/onboarding_howto.md)