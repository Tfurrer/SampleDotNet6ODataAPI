using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Api.Helpers
{
    public static class HttpClientConfigHelper
    {

        public static void ConfigureClient(this HttpClient client, string url, string apiKey = "")
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        }
    }
}
