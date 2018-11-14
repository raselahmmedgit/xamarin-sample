using App1770.AppCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace App1770
{
	public partial class App : Application
	{
        public static App Instance;

        public App ()
		{
			InitializeComponent();
            Instance = this;
            MainPage = new NavigationPage(new MainPage());
        }

        private static Stopwatch _stopWatch = new Stopwatch();
        private const int defaultTimespan = 1;

        protected override void OnStart ()
        {
            // Handle when your app sleeps

            //PullNotification();

            //PullNotificationWithStopwatch();

        }

        private void PullNotification()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                // Logic for logging out if the device is inactive for a period of time.

                InvokeOnMainThread();

                // Always return true as to keep our device timer running.
                return true;
            });
        }

        private void PullNotificationWithStopwatch()
        {
            // On start runs when your application launches from a closed state, 

            if (!_stopWatch.IsRunning)
            {
                _stopWatch.Start();
            }

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                // Logic for logging out if the device is inactive for a period of time.

                if (_stopWatch.IsRunning && _stopWatch.Elapsed.Minutes >= defaultTimespan)
                {
                    //prepare to perform your data pull here as we have hit the 1 minute mark   

                    // Perform your long running operations here.

                    InvokeOnMainThread();

                    _stopWatch.Restart();
                }

                // Always return true as to keep our device timer running.
                return true;
            });
        }

        private void InvokeOnMainThread()
        {
            Task.Run(async () => {
                // If you need to do anything with your UI, you need to wrap it in this.
                await NotificationHelper.PullNotificationAsync();
            });
        }

        protected override void OnSleep ()
		{
            // Handle when your app sleeps
            // On start runs when your application launches from a closed state, 

            if (!_stopWatch.IsRunning)
            {
                _stopWatch.Start();
            }

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                // Logic for logging out if the device is inactive for a period of time.

                if (_stopWatch.IsRunning && _stopWatch.Elapsed.Minutes >= defaultTimespan)
                {
                    //prepare to perform your data pull here as we have hit the 1 minute mark   

                    // Perform your long running operations here.

                    //InvokeOnMainThread(() => {
                    //    // If you need to do anything with your UI, you need to wrap it in this.
                    //});

                    InvokeOnMainThread();

                    _stopWatch.Restart();
                }

                // Always return true as to keep our device timer running.
                return true;
            });
        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
            
        }
	}
}
