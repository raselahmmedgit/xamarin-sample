using System;
using System.Collections.Generic;
using System.Text;

namespace App1770.AppCore
{
    public interface IToast
    {
        void Show(string message);
        void ShowNotification(string title, string message, string id);
    }
}
