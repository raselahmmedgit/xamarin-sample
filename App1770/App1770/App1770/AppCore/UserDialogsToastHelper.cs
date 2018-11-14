using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1770.AppCore
{
    public static class UserDialogsToastHelper
    {
        public static void ShowToastMessage(string message)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(3000);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));

            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
