using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.Net.Http.Headers;
using System.Text;

using FortniteOCR.Models;
using FortniteOCR.Models.Responses;
using FortniteOCR.Services;

namespace FortniteOCR.Clients
{
    internal static class BackendClient
    {
        private static readonly string ENDPOINT_PREFIX = "https://wlspa.josepsalva.name/api/v1";

        public static BackendResponse StoreData(uint observerId, GameDecodedInfo gameDecodedInfo, ILogger<OcrService> logger)
        {
            string rawBody = JsonConvert.SerializeObject(gameDecodedInfo);

            Dictionary<string, string>? body = new()
            {
                { "game_data", rawBody }
            };

            return MakeRequest(HttpMethod.Patch, $"observers/{observerId}", body, logger);
        }

        private static BackendResponse MakeRequest(HttpMethod method, string endpoint, Dictionary<string, string>? data, ILogger<OcrService> logger)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            byte[] hashBytes = Encoding.ASCII.GetBytes(string.Format("{0}", "d5e7a656c9e378874ea28c3c1cc7688f297fa1dc"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Convert.ToBase64String(hashBytes));

            Uri uri = new($"{ENDPOINT_PREFIX}/{endpoint}");

            HttpRequestMessage request = new()
            {
                Method = method,
                RequestUri = uri,
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = client.SendAsync(request).Result;

            string apiResponse = response.Content.ReadAsStringAsync().Result;
            logger.LogDebug($"DEBUG -> {apiResponse}");
            try
            {
                BackendResponse? responseBody = JsonConvert.DeserializeObject<BackendResponse>(apiResponse);
                if (responseBody is null) throw new Exception("Null response body");
                else if (!responseBody.Code.Equals(200)) throw new Exception(responseBody.Message);

                return responseBody;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred while calling the API. It responded with the following message: {e.Message}");
            }
        }
    }
}
