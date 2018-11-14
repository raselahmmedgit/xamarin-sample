using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1770.AppCore
{
    public static class ToastHelper
    {
        public static void ShowToastMessage(string message)
        {
            DependencyService.Get<IToast>().Show(message);
        }

        public async static void ShowPushNotification(string title, string description, string id)
        {
            DependencyService.Get<IToast>().ShowNotification(title, description, id);

            //var iToastNotificator = DependencyService.Get<IToastNotificator>();
            //var notificationOptions = new NotificationOptions
            //{
            //    Title = title,
            //    Description = description,
            //    IsClickable = true,
            //    AndroidOptions = new AndroidOptions
            //    {
            //        ForceOpenAppOnNotificationTap = true
            //    }
            //};

            //INotificationResult iNotificationResult = await iToastNotificator.Notify(notificationOptions);

        }

    }
}
