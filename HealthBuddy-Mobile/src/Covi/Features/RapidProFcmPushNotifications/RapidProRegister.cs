using Newtonsoft.Json;
using System;

namespace Covi.Features.RapidProFcmPushNotifications
{

    [Serializable]
    public class RapidProRegister
    {
        [JsonProperty(PropertyName = "contact_uuid")]
        public string ContactUuid { get; set; }
    }
}
