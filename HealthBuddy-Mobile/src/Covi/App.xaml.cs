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

using Covi.Features;
using Covi.Features.AboutUs;
using Covi.Features.AboutUs.Actions;
using Covi.Features.AccountFeaturesInformation;
using Covi.Features.AppSettings;
using Covi.Features.ChangeCountryProgram;
using Covi.Features.ChangeCountryProgram.Actions;
using Covi.Features.ChangeLanguage;
using Covi.Features.ChangeLanguage.Actions;
using Covi.Features.Chat;
using Covi.Features.CreateProfile;
using Covi.Features.ExpiredToken;
using Covi.Features.Filters;
using Covi.Features.Filters.Actions;
using Covi.Features.ForceUpdate;
using Covi.Features.Main;
using Covi.Features.Maintenance;
using Covi.Features.Menu;
using Covi.Features.NewsArticle;
using Covi.Features.NewsArticle.Actions;
using Covi.Features.Newsfeed;
using Covi.Features.OnBoarding;
using Covi.Features.OnBoarding.Actions;
using Covi.Features.Polls;
using Covi.Features.PrivacyPolicy;
using Covi.Features.PrivacyPolicy.Actions;
using Covi.Features.RapidProFcmPushNotifications;
using Covi.Features.RapidProFcmPushNotifications.Services;
using Covi.Features.Regions;
using Covi.Features.Registry;
using Covi.Features.Rumours;
using Covi.Features.Rumours.Actions;
using Covi.Features.Settings;
using Covi.Features.Settings.Actions;
using Covi.Features.SettingsChangeCountryProgram;
using Covi.Features.SettingsChangeCountryProgram.Actions;
using Covi.Features.SettingsChangeLanguage;
using Covi.Features.SettingsChangeLanguage.Actions;
using Covi.Features.UserDeleteAccount;
using Covi.Features.UserDeleteAccount.Actions;
using Covi.Features.UserLogIn;
using Covi.Logs;
using Covi.Services.Actions;
using Covi.Services.ApplicationMetadata;
using Covi.Services.Feature;
using Covi.Services.Navigation;
using Covi.Services.Serialization;
using DryIoc;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.AppModel;
using Prism.Behaviors;
using Prism.DryIoc.Extensions;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Covi
{
    public partial class App
    {
        private Microsoft.Extensions.Logging.ILogger _logger;

        /*
        * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
        * This imposes a limitation in which the App class must have a default constructor.
        * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
        */
        public App()
            : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        private static bool IsInitialized { get; set; }

        protected override async void OnInitialized()
        {
            try
            {
                if (!IsInitialized)
                {
                    Xamarin.Essentials.VersionTracking.Track();

                    TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

                    Device.SetFlags(new string[] { "Expander_Experimental", "RadioButton_Experimental", "CarouselView_Experimental" });
                }

#if DEBUG
                SetForDebugging(_logger);
#endif

                InitializeComponent();

                var mediator = this.Container.Resolve<IMediator>();
                await mediator.Send(new AppInitializedAction());
                await mediator.Publish(new AppInitializedNotification());

                IsInitialized = true;

                InitializeFcmAndRapidPro();

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: App initialization  failed. Reason: " + ex.ToString());
                Debugger.Break();
            }
        }

        private void InitializeFcmAndRapidPro()
        {
            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;

            RapidProContainer rapidProContainer = new RapidProContainer();

            if (string.IsNullOrEmpty(rapidProContainer.RapidProFcmToken))
            {
                string fcmPushNotificationToken = CrossFirebasePushNotification.Current?.Token;
                rapidProContainer.RapidProFcmToken = fcmPushNotificationToken;

                Console.WriteLine($"App - InitializeFcmAndRapidPro: Token - {fcmPushNotificationToken}");
            }

            if (string.IsNullOrEmpty(rapidProContainer.RapidProUrn))
            {
                string rapidProUrn = RapidProHelper.GetUrnFromGuid();
                rapidProContainer.RapidProUrn = rapidProUrn;

                Console.WriteLine($"App - InitializeFcmAndRapidPro: Urn - {rapidProUrn}");
            }
            
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            RapidProContainer rapidProContainer = new RapidProContainer();

            if (string.IsNullOrEmpty(rapidProContainer.RapidProFcmToken))
            {
                string fcmPushNotificationToken = e.Token;
                rapidProContainer.RapidProFcmToken = fcmPushNotificationToken;

                Console.WriteLine($"App - Current_OnTokenRefresh: {fcmPushNotificationToken}");
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            AppStateContainer.GetInstance().IsAppActive = true;
        }

        protected override void OnResume()
        {
            base.OnResume();

            AppStateContainer.GetInstance().IsAppActive = true;

            // This line of code allows us to avoid bug on iOs when page appearing event doesn't
            // call after restoring app from background. This bug is fixed in upcoming version of
            // Xamarin.Forms 4.8.0 so we should remove this line after update Xamarin.Forms
            // to version 4.8.0 https://github.com/xamarin/Xamarin.Forms/pull/11172
            (MainPage as IPageLifecycleAware)?.OnAppearing();
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            AppStateContainer.GetInstance().IsAppActive = false;

            // This line of code allows us to avoid bug on iOs when page appearing event doesn't
            // call after restoring app from background. This bug is fixed in upcoming version of
            // Xamarin.Forms 4.8.0 so we should remove this line after update Xamarin.Forms
            // to version 4.8.0 https://github.com/xamarin/Xamarin.Forms/pull/11172
            (MainPage as IPageLifecycleAware)?.OnDisappearing();

            try
            {
                var storage = App.Current.Container.Resolve<Services.Storage.IStorageService>();
                storage?.Suspend();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database suspension failed.");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            SetupLogger(containerRegistry);

            if (IsInitialized)
            {
                // Registration was executed in this application session.
                return;
            }

            ConfigureRegionManager();
            ConfigureMainPage();

            containerRegistry.RegisterSingleton<IPageBehaviorFactory, CustomPageBehaviorFactory>();
            containerRegistry.RegisterForNavigation<Features.Shell.ShellPage>();

            containerRegistry.RegisterInstance<Client.Services.PlatformClientOptions>(
                (new Client.Services.PlatformClientOptions(new Uri(Configuration.Constants.PlatformConstants.EndpointUrl))));

            RegisterMediator(containerRegistry);
            RegisterFeatures();
            RegisterNavigationDelegate(containerRegistry);
        }

        private void RegisterMediator(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            container.RegisterDelegate<ServiceFactory>(r => r.Resolve);
            container.Register<IMediator, Mediator>();
            containerRegistry.Register<IMediator, Mediator>();

            container.Register(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>), ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
            container.Register(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>), ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
        }

        /// <summary>
        /// For the detailed information about features approach read docs/modularity/features/features.md.
        /// </summary>
        private void RegisterFeatures()
        {
            this.RegisterFeature<BaseServicesFeature>((provider, feature) =>
                feature.RegisterGeneralPurposeTriggers(
                    provider,
                    MetadataTriggers.ForceFetchMetadataActionHandler));

            this.RegisterFeature<CreateProfileFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    CreateProfileTriggers.Navigation.In.AppInitializedActionHandler,
                    CreateProfileTriggers.Navigation.Out.NavigateToLogInActionHandler,
                    CreateProfileTriggers.Navigation.Out.NavigateToOnBoardingActionHandler));

            this.RegisterFeature<FiltersFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    FiltersTriggers.Navigation.In.NavigateToFiltersActionHandler,
                    FiltersTriggers.Navigation.Out.NavigateToNewsfeed));

            this.RegisterFeature<AccountInformationFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    AccountInformationTriggers.Navigation.In.NavigateToAccountInformationActionHandler,
                    AccountInformationTriggers.Navigation.Out.NavigateCreateProfile,
                    AccountInformationTriggers.Navigation.Out.SignInAnonymous));

            //this.RegisterFeature<MainFeature>((provider, feature) =>
            //    feature.RegisterPageService<NewsfeedPageProviderService>(provider)
            //        .RegisterPageService<PollsPageProviderService>(provider)
            //        .RegisterPageService<ChatTabbedPageProviderService>(provider)
            //        .RegisterPageService<RumoursTabbedPageProviderService>(provider)
            //        .RegisterPageService<MenuPageProviderService>(provider));

            this.RegisterFeature<MainFeature>((provider, feature) =>
                feature.RegisterPageService<NewsfeedPageProviderService>(provider)
                    .RegisterPageService<ChatTabbedPageProviderService>(provider)
                    .RegisterPageService<RumoursTabbedPageProviderService>(provider)
                    .RegisterPageService<MenuPageProviderService>(provider));

            this.RegisterFeature<UserLogInFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    UserLogInTriggers.Navigation.Out.NavigateToOnboardingActionHandler));

            this.RegisterFeature<OnBoardingFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    OnBoardingTriggers.Navigation.Out.NavigateToMainActionHandler));

            this.RegisterFeature<ForceUpdateFeature>();
            this.RegisterFeature<MaintenanceFeature>();
            this.RegisterFeature<ExpiredTokenFeature>();
            this.RegisterFeature<MenuFeature>();


            this.RegisterFeature<NewsArticleFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    NewsArticleTriggers.Navigation.In.NavigateToNewsArticleActionHandler));

            this.RegisterFeature<RumoursFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    RumoursTriggers.Navigation.In.NavigateToRumoursActionHandler));

            this.RegisterFeature<AboutUsFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    AboutUsTriggers.Navigation.In.NavigateToAboutUsActionHandler));

            this.RegisterFeature<SettingsFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    SettingsTriggers.Navigation.In.NavigateToSettingsActionHandler));

            this.RegisterFeature<PrivacyPolicyFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    PrivacyPolicyTriggers.Navigation.In.NavigateToPrivacyPolicyActionHandler));

            this.RegisterFeature<ChangeLanguageFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    ChangeLanguageTriggers.Navigation.In.NavigateToChangeLanguageActionHandler,
                    ChangeLanguageTriggers.Navigation.Out.NavigateToChangeCountryProgramActionHandler));

            this.RegisterFeature<SettingsChangeLanguageFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    SettingsChangeLanguageTriggers.Navigation.In.NavigateToSettingsChangeLanguageActionHandler));

            this.RegisterFeature<ChangeCountryProgramFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    ChangeCountryProgramTriggers.Navigation.In.NavigateToChangeCountryProgramActionHandler,
                    ChangeCountryProgramTriggers.Navigation.Out.NavigateToWelcomeActionHandler));

            this.RegisterFeature<SettingsChangeCountryProgramFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    SettingsChangeCountryProgramTriggers.Navigation.In.NavigateToSettingsChangeCountryProgramActionHandler,
                    SettingsChangeCountryProgramTriggers.Navigation.Out.NavigateToSettingsWelcomeActionHandler));

            this.RegisterFeature<DeleteAccountFeature>((provider, feature) =>
                feature.RegisterNavigationTriggers(
                    provider,
                    DeleteAccountTriggers.Navigation.In.NavigateToDeleteAccountAndDataActionHandler
                    ));

            this.RegisterFeature<ChatFeature>((provider, feature) =>
            {
                feature.RegisterNavigationTriggers(
                    provider,
                    ChatTriggers.Navigation.In.NavigateToChat);
            });

            //this.RegisterFeature<PollsFeature>((provider, feature) =>
            //{
            //    feature.RegisterGeneralPurposeTriggers(
            //        provider,
            //        PollsTriggers.LinkTriggers.PollsLinkHandler);
            //});
        }

        private void RegisterNavigationDelegate(IContainerRegistry container)
        {
            container.RegisterServices((services) =>
                                       {
                                           services.UseNavigationDelegate();
                                       });
        }

        private void SetupLogger(IContainerRegistry containerRegistry)
        {
            var loggerFactory = Logs.Logger.Factory;
            containerRegistry.RegisterInstance<Microsoft.Extensions.Logging.ILoggerFactory>(loggerFactory);
            _logger = loggerFactory.CreateLogger(nameof(Covi.App));
        }

        protected override void ConfigureAggregateLogger(IAggregateLogger aggregateLogger, IContainerProvider container)
        {
            base.ConfigureAggregateLogger(aggregateLogger, container);
            aggregateLogger.AddLogger(new PrismConsoleLogger());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            if (IsInitialized)
            {
                // Registration was executed in this application session.
                return;
            }

            // Application features registrations
            moduleCatalog.AddModule<Features.Account.AccountModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Features.Welcome.WelcomeModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Features.UserProfile.UserProfileModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Features.Newsfeed.NewsfeedModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Features.PushNotifications.PushNotificationsModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<Features.UserLogOut.LogOutModule>(InitializationMode.WhenAvailable);

            // Initialize platform specific modules
            var moduleCatalogInitializer = Container.Resolve<IModuleCatalogInitializer>();
            moduleCatalogInitializer.ConfigureModuleCatalog(moduleCatalog);

            ConfigureOnBoarding(moduleCatalog);
            ConfigureCleanUpBehavior(moduleCatalog);
        }

        // In this method you can register OnBoarding steps which you need.
        // Steps will be shown in the defined order.
        private void ConfigureOnBoarding(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Features.PushNotifications.OnBoardingStep.PushNotificationOnBoardingModule>(InitializationMode.WhenAvailable);
        }

        private void ConfigureCleanUpBehavior(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Services.CleanUp.CleanUpModule>();
            moduleCatalog.AddModule<Features.Account.Handlers.AuthenticationInfoCleanUpHandlerModule>();
            moduleCatalog.AddModule<Services.Storage.Handlers.StorageCleanUpHandlerModule>();
            moduleCatalog.AddModule<Services.Security.Handlers.SecretsCleanUpHandlerModule>();
        }

        private void ConfigureRegionManager()
        {
            RegionManager.SetResolver(() => Container);
        }

        private void ConfigureMainPage()
        {
            MainPageService.SetResolver(() => Container);
        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            if (uri == Covi.Features.DeepLinks.DeepLinks.Polls)
            {
                var mediator = Container.Resolve<IMediator>();
                await mediator.Send(new PollsAction());
            }
        }

        protected override void OnNavigationError(Prism.Events.INavigationError navigationError)
        {
            var parameters = navigationError.Parameters is null ? string.Empty : Serializer.Instance.SerializeAsync(navigationError.Parameters).GetAwaiter().GetResult();
            _logger.LogError(navigationError.Exception, $"Navigation Error: uri: {navigationError.NavigationUri}, args: {parameters}");
            base.OnNavigationError(navigationError);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            _logger.LogError(args.Exception, "Exception: " + args.Exception.Message);
        }

        protected override void TrackError(Exception ex, string fromEvent, object errorObject = null)
        {
            if (errorObject != null)
            {
                System.Diagnostics.Debug.WriteLine(errorObject);
                _logger.LogError(ex, "Exception: " + errorObject.ToString());
            }
            else
            {
                _logger.LogError(ex, "Exception: from event: " + fromEvent);
            }

            base.TrackError(ex, fromEvent, errorObject);
        }

#if DEBUG
        private void SetForDebugging(Microsoft.Extensions.Logging.ILogger logger)
        {
            Xamarin.Forms.Internals.Log.Listeners.Add(new DebugListener(logger));
        }

        private class DebugListener : Xamarin.Forms.Internals.LogListener
        {
            private readonly Microsoft.Extensions.Logging.ILogger _logger;

            public DebugListener(Microsoft.Extensions.Logging.ILogger logger)
            {
                _logger = logger;
            }

            public override void Warning(string category, string message)
            {
                _logger.LogDebug($"{category}: {message}");
            }
        }
#endif
    }
}
