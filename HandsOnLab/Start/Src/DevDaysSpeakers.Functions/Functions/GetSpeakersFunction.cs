using System.Collections.Generic;
using DevDaysSpeakers.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DevDaysSpeakers.Functions
{
    public static class GetSpeakersFunction
    {
        [FunctionName(nameof(GetSpeakersFunction))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            //Add logic to return a 200 OK Response with a JSON body containing the list of speakers in the body
			return null;
        }

        static List<Speaker> GenerateSpeakers()
        {
            //Generate a list of speakers
			return null;
        }
    }
}
