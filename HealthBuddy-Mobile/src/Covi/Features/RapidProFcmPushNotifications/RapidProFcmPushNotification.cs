using Newtonsoft.Json;
using System;

namespace Covi.Features.RapidProFcmPushNotifications
{
    [Serializable]
    public class RapidProFcmPushNotification
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "message_id")]
        public string MessageId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "quick_replies")]
        public string QuickReplies { get; set; }
    }

    [Serializable]
    public class RapidProFcmPushNotificationQuickReplie
    {
        [JsonProperty(PropertyName = "message_id")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Value { get; set; }
    }
}
