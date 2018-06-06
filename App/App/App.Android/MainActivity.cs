using System;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Java.Lang;
using String = System.String;
using NotificationCompat = Android.Support.V4.App.NotificationCompat;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Android.Gms.Common;
using Android.Util;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace App.Droid
{
    [Activity(Label = "Push Notification", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // Unique ID for our notification: 
        private static readonly int ButtonClickNotificationId = 1000;

        // Number of times the button is tapped (starts with first tap):
        private int count = 1;

        TextView textViewRemotePushNotification;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btnLocalPushNotification);

            button.Click += ButtonOnClick;

            //ListView listView = FindViewById<ListView>(Resource.Id.listViewStudent);

            var dataList = GetStudentList();

            //listView.Adapter = GetStudentList();

            //IsPlayServicesAvailable();
        }

        #region Local
        // Handler for button click events.
        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            // Pass the current button press count value to the next activity:
            Bundle valuesForActivity = new Bundle();
            valuesForActivity.PutInt("count", count);

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(this, typeof(SecondActivity));

            // Pass some values to SecondActivity:
            resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Class.FromType(typeof(SecondActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetAutoCancel(true)                    // Dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(resultPendingIntent)  // Start up this activity when the user clicks the intent.
                .SetContentTitle("Button Clicked")      // Set the title
                .SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.Icon) // This is the icon to display
                .SetContentText(String.Format("The button has been clicked {0} times.", count)); // the message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(ButtonClickNotificationId, builder.Build());

            // Increment the button press count:
            count++;
        }

        #endregion

        #region Remote
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    textViewRemotePushNotification.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    textViewRemotePushNotification.Text = "Sorry, this device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                textViewRemotePushNotification.Text = "Google Play Services is available.";
                return true;
            }
        }
        #endregion

        #region Api Call

        private List<Student> GetStudentList()
        {
            List<Student> studentList = new List<Student>();

            string url = "http://localhost:47641/api/default";

            var request = HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                }
                
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }
                }
            }

            return studentList;
        }

        #endregion
    }
}


