﻿using System.Text.Json.Serialization;

namespace RequestProcessor.App.Models.Impl
{
    internal class RequestOptions : IRequestOptions, IResponseOptions
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("method")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestMethod Method { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        public bool IsValid { get; set; }


        public RequestOptions()
        {
        }
    }
}
