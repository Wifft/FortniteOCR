using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace FortniteOCR.Models.Responses
{
    internal sealed class BackendResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
