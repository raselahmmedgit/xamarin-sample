using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App1770.AppCore
{
    public static class NotificationHelper
    {
        public static async Task PullNotificationAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var notificationViewModelList = await GetNotifications();
                foreach (var notificationViewModel in notificationViewModelList)
                {
                    ToastHelper.ShowPushNotification(notificationViewModel.NotificationTitle, notificationViewModel.NotificationDescription, notificationViewModel.NotificationId.ToString());
                }
            }
            else
            {

            }
        }

        private static string token = "";
        private static string tokenType = "";
        private static string notificationUri = "/api/PatientProfile/Notifications";

        private static async Task<List<NotificationViewModel>> GetNotifications()
        {
            List<NotificationViewModel> notificationViewModelList = new List<NotificationViewModel>();
            try
            {
                #region Api Call

                var restUrl = AppConstant.BaseAddress + notificationUri;
                var uri = new Uri(string.Format(restUrl, string.Empty));
                var httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = 256000;
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token, tokenType);
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    notificationViewModelList = JsonConvert.DeserializeObject<List<NotificationViewModel>>(result);
                }

                #endregion

                notificationViewModelList = new List<NotificationViewModel>()
                    {
                        new NotificationViewModel { NotificationId=1, NotificationTitle="Notification 1", NotificationDescription="Notification Description 1" },
                        new NotificationViewModel { NotificationId=2, NotificationTitle="Notification 2", NotificationDescription="Notification Description 2" },
                        new NotificationViewModel { NotificationId=3, NotificationTitle="Notification 3", NotificationDescription="Notification Description 3" }
                    };

            }
            catch (Exception ex)
            {

            }
            return notificationViewModelList;
        }
    }
}
