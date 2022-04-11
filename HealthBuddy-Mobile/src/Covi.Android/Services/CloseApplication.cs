using Android.App;
using Covi.Services;

namespace Covi.Droid.Services
{
    public class CloseApplication : ICloseApplication
    {
        public void Close()
        {
            var activity = (Activity)Xamarin.Forms.Forms.Context;
            activity.FinishAffinity();
        }
    }
}
