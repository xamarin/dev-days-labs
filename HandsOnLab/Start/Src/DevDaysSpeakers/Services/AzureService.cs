﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using DevDaysSpeakers.Shared.Models;

using Newtonsoft.Json;

namespace DevDaysSpeakers.Services
{
    public static class AzureService
    {
        const string getSpeakersFunctionUrl = "[Your Azure Functions URL]";

        readonly static Lazy<HttpClient> clientHolder = new Lazy<HttpClient>(() => new HttpClient());

        static HttpClient Client => clientHolder.Value;

        public static async Task<List<Speaker>> GetSpeakers()
        {
            var httpResponseMessage = await Client.GetAsync(getSpeakersFunctionUrl);

            httpResponseMessage.EnsureSuccessStatusCode();

            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Speaker>>(json);
        }
    }
}