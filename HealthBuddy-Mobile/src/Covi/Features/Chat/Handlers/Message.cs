// =========================================================================
// Copyright 2020 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================

using Newtonsoft.Json;
using System;

namespace Covi.Features.Chat.Handlers
{
    [Serializable]
    public class Message
    {
        /// <summary>
        /// The name of the event of interaction between native code and page js code.
        /// </summary>
        [JsonProperty(PropertyName = "eventName")]
        public string EventName { get; set; }

        /// <summary>
        /// A base64 string with serialized payload.
        /// </summary>
        [JsonProperty(PropertyName = "payload")]
        public string Payload { get; set; }
    }

    [Serializable]
    public class InitPayload
    {
        [JsonProperty(PropertyName = "initPhrase")]
        public string InitPhrase { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
    }

    [Serializable]
    public class SendTextPayload
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

    [Serializable]
    public class StatusPayload
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
    
}
