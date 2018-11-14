using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1770.AppCore
{
    public static class LoaderHelper
    {
        public static void ShowLoader(StackLayout loaderStackLayout)
        {
            loaderStackLayout.IsVisible = true;
        }

        public static void HideLoader(StackLayout loaderStackLayout)
        {
            loaderStackLayout.IsVisible = false;
        }
    }
}
