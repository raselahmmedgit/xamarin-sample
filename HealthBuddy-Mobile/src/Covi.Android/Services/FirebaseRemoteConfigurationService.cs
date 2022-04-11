using Covi.Features.FirebaseRemoteConfig;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covi.Droid.Services
{
    public class FirebaseRemoteConfigurationService : IFirebaseRemoteConfigurationService
    {
        public FirebaseRemoteConfigurationService()
        {
            FirebaseRemoteConfigSettings configSettings = new FirebaseRemoteConfigSettings.Builder()
                .SetDeveloperModeEnabled(true)
                .Build();
            FirebaseRemoteConfig.Instance.SetConfigSettings(configSettings);
        }

        public async Task FetchAndActivateAsync()
        {
            try
            {
                await FirebaseRemoteConfig.Instance.FetchAsync(0);
                FirebaseRemoteConfig.Instance.ActivateFetched();
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
                var setting = FirebaseRemoteConfig.Instance.GetString(key);
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
                var settings = FirebaseRemoteConfig.Instance.GetString(key);
                return await Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<T>>(settings));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
