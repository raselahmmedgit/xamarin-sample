using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using App1770.AppCore;
using App1770.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(iOSToast))]
namespace App1770.iOS
{
    public class iOSToast : IToast
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void Show(string message)
        {
            alertDelay = NSTimer.CreateScheduledTimer(SHORT_DELAY, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(AppConstant.ToastMessageTitle, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        public void ShowNotification(string title, string message, string id)
        {
            throw new NotImplementedException();
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }

    }
}