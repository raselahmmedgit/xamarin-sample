using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Covi.Features.FirebaseRemoteConfig
{
    [Serializable]
    public class FirebaseRemoteConfiguration
    {
        [JsonProperty(PropertyName = "is_active")]
        public string IsActive { get; set; }

        [JsonProperty(PropertyName = "channel_name")]
        public string ChannelName { get; set; }

        [JsonProperty(PropertyName = "channel_host")]
        public string ChannelHost { get; set; }

        [JsonProperty(PropertyName = "channel_id")]
        public string ChannelId { get; set; }
    }

}
