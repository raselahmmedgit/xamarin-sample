using Covi.Features.FirebaseRemoteConfig;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covi.iOS.Services
{
    public class FirebaseRemoteConfigurationService : IFirebaseRemoteConfigurationService
    {
        public FirebaseRemoteConfigurationService()
        {
            RemoteConfig.SharedInstance.SetDefaults("RemoteConfigDefaults");
            RemoteConfig.SharedInstance.ConfigSettings = new RemoteConfigSettings(true);
        }

        public async Task FetchAndActivateAsync()
        {
            try
            {
                var status = await RemoteConfig.SharedInstance.FetchAsync(0);
                if (status == RemoteConfigFetchStatus.Success)
                {
                    RemoteConfig.SharedInstance.ActivateFetched();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var setting = RemoteConfig.SharedInstance[key].StringValue;
                return await Task.FromResult(JsonConvert.DeserializeObject<T>(setting));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string key)
        {
            try
            {
                var settings = RemoteConfig.SharedInstance[key].StringValue;
                return await Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<T>>(settings));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
