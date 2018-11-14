using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Xamarin.Forms;

namespace App1820.Droid
{
    [Activity(Label = "App1820", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            #region MyRegion


            Intent intent = this.Intent;
            Bundle notificationBundle = intent.GetBundleExtra("notification");

            // only show when app first start
            //if (notificationBundle == null)
            //{
            //    Util.StartPowerSaverIntent(this);
            //}

            //start service to keep app run always
            Droid.NotificationService.baseUrl = AppConstant.BaseUrl;
            Droid.NotificationService.IntervaInlMinute = (long)AppConstant.IntervaInlMinute;
            Droid.NotificationService.apiToken = AppConstant.ApiToken;
            Intent serviceIntent = new Intent(this, typeof(BackgroudService));

            StartService(serviceIntent);

            // open notification schecule through notifiaiton
            if (notificationBundle != null)
            {
                string notificationId = notificationBundle.GetString("notificationId");

                string patientProcedureDetailId = notificationBundle.GetString("patientProcedureDetailId");

                if (notificationId != null && patientProcedureDetailId != null)
                {
                    try
                    {
                        var schedulePage = new SchedulePage(notificationId, patientProcedureDetailId);
                        //Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(schedulePage);
                        //Xamarin.Forms.Application.Current.MainPage = new SchedulePage(notificationId, patientProcedureDetailId); //ok

                        //Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(schedulePage));

                        Xamarin.Forms.Application.Current.MainPage = new NavigationPage(schedulePage); //ok

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            #endregion
        }
    }
}