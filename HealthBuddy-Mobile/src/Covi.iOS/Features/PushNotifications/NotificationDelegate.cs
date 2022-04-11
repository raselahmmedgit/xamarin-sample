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
using Covi.Features.AppSettings;
using Covi.Features.PushNotifications;
using Covi.Features.PushNotifications.Services;

using Foundation;

using UserNotifications;

namespace Covi.iOS.Features.PushNotifications
{
    public class NotificationDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);

            AppPreferences.Instance.IsAppNeedsForceRefreshUserStatus = true;
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            if (response.IsDefaultAction)
            {
                ProcessNotification(response.Notification?.Request?.Content?.UserInfo);
            }

            completionHandler();
        }

        public static void ProcessNotification(NSDictionary userInfo)
        {
            var notification = ParseNotification(userInfo);

            if (notification != null)
            {
                PushNotificationsManager.Instance.HandleAsync(notification).FireAndForget();
            }
        }

        private static PushNotification ParseNotification(NSDictionary userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            var notification = new PushNotification();

            notification.Type = userInfo.GetStringValue("type");
            if (userInfo["aps"] is NSDictionary aps)
            {
                if (aps["alert"] is NSDictionary alert)
                {
                    notification.Title = alert.GetStringValue("title");
                    notification.SubTitle = alert.GetStringValue("subtitle");
                    notification.Description = alert.GetStringValue("body");
                }
            }

            return notification;
        }
    }
}
