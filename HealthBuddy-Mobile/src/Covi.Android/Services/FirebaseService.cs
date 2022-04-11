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

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Covi.Features.PushNotifications;
using Covi.Features.PushNotifications.Models;
using Covi.Features.PushNotifications.Services;
using Firebase.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using Covi.Features.AppSettings;
using NotificationManager = Android.App.NotificationManager;
using Covi.Services.Serialization;
using Newtonsoft.Json;
using Covi.Features.RapidProFcmPushNotifications;

namespace Covi.Droid.Services
{
    [Service]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService : FirebaseMessagingService
    {
        private readonly ILogger _logger;

        public FirebaseService()
        {
            _logger = Covi.Logs.Logger.Get(this);
        }

        public override void OnNewToken(string token)
        {
            SendPushTokenToServer(token);
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            System.Diagnostics.Debug.WriteLine("Push notification received.");

            var pushNotification = ParseRemoteMessage(message);

            var rapidProFcmPushNotification = ParseRapidProRemoteMessage(message);

            // convert the incoming message to a local notification
            SendNotification(pushNotification);

            AppPreferences.Instance.IsAppNeedsForceRefreshUserStatus = true;
        }

        private void SendNotification(PushNotification notificationDetails)
        {
            var notificationManager = NotificationManager.FromContext(this);

            var channelId = Covi.Configuration.Constants.PushNotificationsConstants.NotificationChannelName;

            var notificationId = new Random().Next();
            var largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icon);
            var notificationBuilder =
                new NotificationCompat.Builder(this, channelId)
                    .SetSmallIcon(Resource.Drawable.notification_icon)
                    .SetLargeIcon(largeIcon)
                    .SetPriority(NotificationCompat.PriorityHigh)
                    .SetContentIntent(BuildIntentToShowMainActivity(notificationDetails))
                    .SetAutoCancel(true);

            if (!string.IsNullOrEmpty(notificationDetails.Title))
            {
                notificationBuilder.SetContentTitle(notificationDetails.Title);
            }

            if (!string.IsNullOrEmpty(notificationDetails.SubTitle))
            {
                notificationBuilder.SetSubText(notificationDetails.SubTitle);
            }

            if (!string.IsNullOrEmpty(notificationDetails.Description))
            {
                notificationBuilder.SetContentText(notificationDetails.Description);
                notificationBuilder.SetStyle(new NotificationCompat.BigTextStyle().BigText(notificationDetails.Description));
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(Covi.Configuration.Constants.PushNotificationsConstants.NotificationChannelName);
            }

            notificationManager.Notify(notificationId, notificationBuilder.Build());
        }

        private PendingIntent BuildIntentToShowMainActivity(PushNotification notificationDetails)
        {
            Intent notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.AddFlags(ActivityFlags.ClearTop);
            notificationIntent.AddFlags(ActivityFlags.SingleTop);

            var serializedNotificationData = AsyncHelpers.RunSync(() => Serializer.Instance.SerializeAsync(notificationDetails));
            notificationIntent.PutExtra(MainActivity.PushNotificationKey, serializedNotificationData);

            PendingIntent pendingIntent = PendingIntent.GetActivity(
                this, 0,
                notificationIntent, 0);
            return pendingIntent;
        }

        private void SendPushTokenToServer(string token)
        {
            try
            {
                PushNotificationsContainer.Instance.SetAsync(new PushNotificationTokenModel { Token = token, Type = "fcm" });
            }
            catch (Exception ex)
            {
                Debugger.Break();
                _logger.LogError(ex, "Failed to send push notification token to backend");
            }
        }

        private PushNotification ParseRemoteMessage(RemoteMessage message)
        {
            string title = null;
            string body = null;

            var notification = message.GetNotification();
            if (notification != null)
            {
                title = notification.Title;
                body = notification.Body;
            }

            if (message.Data?.Any() != true)
            {
                return new PushNotification
                {
                    Title = title,
                    Description = body
                };
            }

            message.Data.TryGetValue("title", out var title1);
            if (!string.IsNullOrEmpty(title1))
            {
                title = title1;
            }

            message.Data.TryGetValue("subtitle", out var subTitle);
            message.Data.TryGetValue("type", out var type);

            message.Data.TryGetValue("payload", out var body1);
            if (!string.IsNullOrEmpty(body1))
            {
                body = body1;
            }

            if (string.IsNullOrEmpty(body))
            {
                message.Data.TryGetValue("message", out var body2);
                if (!string.IsNullOrEmpty(body2))
                {
                    body = body2;
                }
            }

            return new PushNotification
            {
                Title = title,
                Description = body,
                SubTitle = subTitle,
                Type = type
            };
        }

        private RapidProFcmPushNotification ParseRapidProRemoteMessage(RemoteMessage message)
        {
            string title = null;
            string body = null;

            var notification = message.GetNotification();
            if (notification != null)
            {
                title = notification.Title;
                body = notification.Body;
            }

            if (message.Data?.Any() != true)
            {
                return new RapidProFcmPushNotification
                {
                    Title = title,
                    Body = body
                };
            }

            message.Data.TryGetValue("title", out var titleData);
            message.Data.TryGetValue("body", out var bodyData);
            message.Data.TryGetValue("type", out var typeData);
            message.Data.TryGetValue("message_id", out var messageIdData);
            message.Data.TryGetValue("message", out var messageData);
            message.Data.TryGetValue("quick_replies", out var quickRepliesData);

            return new RapidProFcmPushNotification
            {
                Title = titleData,
                Type = typeData,
                Body = bodyData,
                MessageId = messageIdData,
                Message = messageData,
                QuickReplies = quickRepliesData
            };
        }
    }
}
