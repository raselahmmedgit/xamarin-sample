using System;
using Xamarin.Forms;

namespace Covi.Features.RapidProFcmPushNotifications.Services
{
    public class FirebaseContainer
    {
        #region Global Variable Declaration

        private const string FirebaseChannelHostKey = "FirebaseChannelHost";
        private const string FirebaseChannelIdKey = "FirebaseChannelId";

        #endregion

        #region Constructor

        #endregion

        #region Actions

        public string FirebaseChannelHost
        {
            get
            {
                return Application.Current.Properties.ContainsKey(FirebaseChannelHostKey) ? Application.Current.Properties[FirebaseChannelHostKey].ToString() : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[FirebaseChannelHostKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        public string FirebaseChannelId
        {
            get
            {
                return Application.Current.Properties.ContainsKey(FirebaseChannelIdKey) ? Application.Current.Properties[FirebaseChannelIdKey].ToString() : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    Application.Current.Properties[FirebaseChannelIdKey] = value;
                    Application.Current.SavePropertiesAsync();
                }
            }
        }

        //public void ClearFirebaseContainer()
        //{
        //    Application.Current.Properties.Clear();
        //    Application.Current.SavePropertiesAsync();
        //}

        #endregion
    }
}
