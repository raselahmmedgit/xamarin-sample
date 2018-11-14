using App1770.AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1770.UWP
{
    public class UWPToast : IToast
    {
        public async void Show(string message)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = AppConstant.ToastMessageTitle,
                Content = message,
                CloseButtonText = AppConstant.ToastMessageButtonText
            };

            ContentDialogResult contentDialogResult = await contentDialog.ShowAsync();
        }

        public void ShowNotification(string title, string message, string id)
        {
            throw new NotImplementedException();
        }
    }
}
