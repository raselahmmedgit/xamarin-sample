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
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Covi.Configuration;
using Covi.Droid.Configuration;
using Covi.Droid.Features.Analytics;
using Covi.Droid.Features.PushNotifications;
using Covi.Droid.Services;
using Covi.Droid.Services.Http;
using Covi.Features;
using Covi.Features.FirebaseRemoteConfig;
using Covi.Features.PushNotifications;
using Covi.Features.PushNotifications.Services;
using Covi.Services;
using Covi.Services.Platform;
using Covi.Services.Serialization;
using Covi.Services.Storage;
using FFImageLoading.Forms.Platform;
using Firebase;
using Microsoft.AppCenter;
using Mobile.BuildTools.Configuration;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using Prism.Modularity;

namespace Covi.Droid
{
    [Activity(Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string PushNotificationKey = nameof(PushNotificationKey);

        protected override void OnCreate(Bundle savedInstanceState)
        {
#if DEBUG
            global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            SetupAnalytics();

            SetupAppCenterAndLogger();

            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            SetupFirebasePushNotification();

            base.OnCreate(savedInstanceState);

            ConfigurationManager.Init(true, this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Expander_Experimental", "RadioButton_Experimental", "CarouselView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App(new PlatformInitializer()));

            if (IsPlayServiceAvailable())
            {
                //CreateNotificationChannel();
            }

            //CheckParameters(Intent);

        }

        private void SetupFirebasePushNotification()
        {
            //FirebaseApp.InitializeApp(this);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";

                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
            }

            FirebasePushNotificationManager.Initialize(this, false);

        }

        private void SetupAppCenterAndLogger()
        {
            if (!string.IsNullOrWhiteSpace(Constants.AppCenterConstants.Secret_Android))
            {
                AppCenter.Start(Constants.AppCenterConstants.Secret_Android,
                    typeof(Microsoft.AppCenter.Analytics.Analytics),
                    typeof(Microsoft.AppCenter.Crashes.Crashes));
                Covi.Logs.Logger.Factory.AddProvider(new Covi.Droid.Services.Log.AppCenterLogProvider(Microsoft.Extensions.Logging.LogLevel.Warning));
            }
        }

        private void SetupAnalytics()
        {
#if !DEBUG
            Covi.Features.Analytics.AnalyticsProvider.SetProvider(new FirebaseAnalyticsService());
#endif
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
            //CheckParameters(intent);
        }

        private async void CheckParameters(Intent intent)
        {
            var pushNotificationData = intent?.Extras?.GetString(PushNotificationKey);
            if (!string.IsNullOrEmpty(pushNotificationData))
            {
                var pushNotification = await Serializer.Instance.DeserializeAsync<PushNotification>(pushNotificationData);

                await PushNotificationsManager.Instance.HandleAsync(pushNotification);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        bool IsPlayServiceAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    System.Diagnostics.Debug.WriteLine(GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("This device is not supported");
                }
                return false;
            }
            return true;
        }

        void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = Covi.Configuration.Constants.PushNotificationsConstants.NotificationChannelName;
                var channelDescription = string.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }


    public class PlatformInitializer : IPlatformInitializer, IModuleCatalogInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterDelegate<IConfigurationManager>(() => ConfigurationManager.Current);
            var droidConfiguration = new DroidConfiguration(ConfigurationManager.Current);
            containerRegistry.RegisterInstance<IEnvironmentConfiguration>(droidConfiguration);
            Constants.Initialize(droidConfiguration);

            containerRegistry.RegisterInstance<IModuleCatalogInitializer>(this);

            containerRegistry.Register<Covi.Services.Http.IHttpClientHandlerProvider, Services.Http.NativeHttpClientHandlerProvider>();
            containerRegistry.Register<IPushNotificationInitializer, PushNotificationPermissionInitializer>();
            containerRegistry.Register<IAppStoreService, AppStoreService>();

            containerRegistry.Register<IFirebaseRemoteConfigurationService, FirebaseRemoteConfigurationService>();

            containerRegistry.Register<ICloseApplication, CloseApplication>();

            HttpHandlersContainer.Instance.AddHandler(() => new AndroidClientConnectivityErrorHandler());
        }

        public void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }
    }
}
