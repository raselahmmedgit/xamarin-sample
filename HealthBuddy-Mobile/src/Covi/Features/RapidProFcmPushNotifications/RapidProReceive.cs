using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Covi.Features.RapidProFcmPushNotifications
{
    [Serializable]
    public class RapidProReceive
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<RapidProReceiveData> Data { get; set; }
    }

    [Serializable]
    public class RapidProReceiveData
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "channel_uuid")]
        public string ChannelUuid { get; set; }

        [JsonProperty(PropertyName = "msg_uuid")]
        public string MsgUuid { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "urn")]
        public string Urn { get; set; }

        [JsonProperty(PropertyName = "received_on")]
        public string ReceivedOn { get; set; }
    }
}
