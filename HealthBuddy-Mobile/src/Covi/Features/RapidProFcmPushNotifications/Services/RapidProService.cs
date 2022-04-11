using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Covi.Features.RapidProFcmPushNotifications.Services
{
    public class RapidProService
    {
        #region Global Variable Declaration

        private HttpClient InitializeHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 556000;
            return httpClient;
        }
        private readonly FirebaseContainer _firebaseContainer;

        #endregion

        #region Constructor

        public RapidProService()
        {
            _firebaseContainer = new FirebaseContainer();
        }

        #endregion

        #region Actions

        public async Task<RapidProRegister> RapidProRegister(string rapidProUrn, string rapidProFcmToken)
        {
            RapidProRegister rapidProRegister = new RapidProRegister();
            try
            {
                var values = new Dictionary<string, string>
                {
                   { "urn", rapidProUrn + string.Empty },
                   { "fcm_token", rapidProFcmToken}
                };
                var content = new FormUrlEncodedContent(values);

                var restUrl = _firebaseContainer.FirebaseChannelHost + _firebaseContainer.FirebaseChannelId + RapidProConstant.RapidProFcmRegister + "?urn=" + rapidProUrn + "&fcm_token=" + rapidProFcmToken;
                var absoluteUrl = restUrl;

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(absoluteUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        rapidProRegister = JsonConvert.DeserializeObject<RapidProRegister>(result);

                        return rapidProRegister;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return rapidProRegister;
        }

        public async Task<RapidProReceive> RapidProReceive(string rapidProUrn, string rapidProFcmToken, string rapidProMsg)
        {
            RapidProReceive rapidProReceive = new RapidProReceive();
            try
            {
                var values = new Dictionary<string, string>
                {
                   { "from", rapidProUrn + string.Empty },
                   { "msg", rapidProMsg + string.Empty },
                   { "fcm_token", rapidProFcmToken}
                };
                var content = new FormUrlEncodedContent(values);

                var restUrl = _firebaseContainer.FirebaseChannelHost + _firebaseContainer.FirebaseChannelId + RapidProConstant.RapidProFcmReceive + "?from=fcm:" + rapidProUrn + "&msg=" + rapidProMsg + "&fcm_token=" + rapidProFcmToken;
                var absoluteUrl = restUrl;

                using (var httpClient = InitializeHttpClient())
                {
                    var response = await httpClient.PostAsync(absoluteUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        rapidProReceive = JsonConvert.DeserializeObject<RapidProReceive>(result);

                        return rapidProReceive;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return rapidProReceive;
        }

        #endregion
    }
}
