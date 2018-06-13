using App1710.ApiHelper;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace App1710
{
	public partial class App : Application
	{
        private readonly ITokenContainer _iTokenContainer;

        public App ()
		{
			InitializeComponent();

            _iTokenContainer = new TokenContainer();

            if (!_iTokenContainer.IsApiCurrentToken())
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
