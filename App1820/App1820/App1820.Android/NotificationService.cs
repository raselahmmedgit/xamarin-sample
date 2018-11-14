using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;

namespace App1820.Droid
{
    [BroadcastReceiver]
    public class NotificationService : BroadcastReceiver
    {
        public static string apiToken = "";
        public static string baseUrl = "";
        public static long IntervaInlMinute = AppConstant.IntervaInlMinute; //default 2 minute
        public static bool isUserLoggedIn = false;
        public override void OnReceive(Context context, Intent intent)
        {
            Bundle bundle = intent.GetBundleExtra("notification");

            if (!isUserLoggedIn)
            {
                isUserLoggedIn = (bundle != null ? bundle.GetBoolean("isUserLoggedIn") : false);
                if (!isUserLoggedIn)
                {
                    isUserLoggedIn = true;
                    //return;
                }
            }

            string apiTokenKey = apiToken;
            if (apiTokenKey == null || apiTokenKey.Length <= 0)
            { apiTokenKey = (bundle != null ? bundle.GetString("apiToken") : ""); }
            string requestBaseUrl = (bundle != null ? bundle.GetString("baseUrl") : baseUrl);
            if (apiTokenKey == null || apiTokenKey.Length <= 0)
            {
                apiTokenKey = AppConstant.ApiToken;
                //return;
            }

            ShowNotification(context, apiTokenKey, requestBaseUrl);
        }
        public void SetAlarm(Context context, Intent intent)
        {
            int requestCode = 0;
            //CancelAlarmIfExists(context, requestCode);


            AlarmManager am = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent alarmIntent = new Intent(context, typeof(NotificationService));
            Bundle bundle = new Bundle();
            bundle.PutString("apiToken", apiToken);
            bundle.PutString("baseUrl", baseUrl);
            bundle.PutBoolean("isUserLoggedIn", isUserLoggedIn);
            alarmIntent.PutExtra("notification", bundle);

            PendingIntent pi = PendingIntent.GetBroadcast(context, requestCode, alarmIntent, 0);
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long time = (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
            am.SetRepeating(AlarmType.RtcWakeup, time, 1000 * 60 * IntervaInlMinute, pi); // Millisec * Second * Minute
        }

        public void CancelAlarmIfExists(Context context, int requestCode)
        {
            try
            {
                Intent intent = new Intent(context, typeof(NotificationService));
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, requestCode, intent, 0);
                AlarmManager am = (AlarmManager)context.GetSystemService(Context.AlarmService);
                am.Cancel(pendingIntent);
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
            }
        }

        public async void ShowNotification(Context context, string apiTokenKey, string baseUrl)
        {
            PatientEmailNotificationHistory patientEmailNotificationHistory = null;
            patientEmailNotificationHistory = GetPatientEmailNotificationHistory(apiTokenKey, baseUrl);

            if (patientEmailNotificationHistory == null || string.IsNullOrEmpty(patientEmailNotificationHistory.MobileTemplateTitle) || string.IsNullOrEmpty(patientEmailNotificationHistory.MobileTemplateDetail))
            {
                return;
            }

            //await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new PhysicianPage());
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            Bundle bundle = new Bundle();
            bundle.PutString("notificationId", patientEmailNotificationHistory.NotificationId + "");
            bundle.PutString("patientProcedureDetailId", patientEmailNotificationHistory.PatientProcedureDetailId + "");
            intent.PutExtra("notification", bundle);
            //intent.SetComponent(new ComponentName(Android.App.Application.Context,typeof(MainActivity)));
            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):

            const int pendingIntentId = 10;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(context, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

            string CHANNEL_ID = "App1820_channel_01";// The id of the channel. 
            ICharSequence name = new Java.Lang.String("App1820");// The user-visible name of the channel.

            // Instantiate the builder and set notification elements, including pending intent:
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetDefaults(NotificationDefaults.Sound)
                .SetContentTitle(patientEmailNotificationHistory.MobileTemplateTitle)
                .SetContentText(patientEmailNotificationHistory.MobileTemplateDetail)
                .SetSmallIcon(Resource.Drawable.icon);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                builder.SetChannelId(CHANNEL_ID);
            }
            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel mChannel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }

            // Publish the notification:
            const int notificationManagerNotifyId = 0;
            notificationManager.Notify(notificationManagerNotifyId, notification);
        }

        private PatientEmailNotificationHistory GetPatientEmailNotificationHistory(string apiTokenKey, string baseUrl)
        {
            PatientEmailNotificationHistory patientEmailNotificationHistory = null;
            try
            {
                patientEmailNotificationHistory = new PatientEmailNotificationHistory()
                {
                    NotificationId = AppConstant.NotificationId,
                    NotificationTitle = "Hello! This is rasel ahmmed.",
                    NotificationDate = DateTime.Now,
                    PatientProcedureDetailId = Guid.NewGuid(),
                    MobileTemplateTitle = "Hello! This is rasel ahmmed.",
                    MobileTemplateDetail = "For Android...!!!"
                };

                return patientEmailNotificationHistory;

                //Android.Widget.Toast.MakeText(Android.App.Application.Context,"success: "+response.StatusCode+" "+apiTokenKey, ToastLength.Short).Show();
            }
            catch (Java.Lang.Exception ex)
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, "failed: " + ex.Message, ToastLength.Short).Show();

                ex.PrintStackTrace();
            }
            return patientEmailNotificationHistory;
        }
    }

    partial class PatientEmailNotificationHistory
    {
        public long NotificationId { get; set; }

        public string NotificationTitle { get; set; }

        public DateTime? NotificationDate { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public string MobileTemplateTitle { get; set; }

        public string MobileTemplateDetail { get; set; }

    }
}