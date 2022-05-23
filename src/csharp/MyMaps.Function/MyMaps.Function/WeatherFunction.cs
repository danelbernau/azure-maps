using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace MyMaps.Function
{
    public static class WeatherFunction
    {

        static HttpClient httpClient = new HttpClient();

        static readonly string subscriptionKey = Environment.GetEnvironmentVariable("MapsSubscriptionKey", EnvironmentVariableTarget.Process);
        static readonly string currentConditionsEndpoint = "https://atlas.microsoft.com/weather/currentConditions/json?api-version=1.0&query={0}&subscription-key=" + subscriptionKey;
        static readonly string dailyForecastEndpoint = "https://atlas.microsoft.com/weather/forecast/daily/json?api-version=1.1&query={0}&subscription-key=" + subscriptionKey;
        static readonly string hourlyForecastEndpoint = "https://atlas.microsoft.com/weather/forecast/hourly/json?api-version=1.1&query={0}&subscription-key=" + subscriptionKey;
        static readonly string minuteByMibuteForecastEndpoint = "https://atlas.microsoft.com/weather/forecast/minute/json?api-version=1.1&query={0}&subscription-key=" + subscriptionKey;

        [FunctionName("current")]
        public static async Task<IActionResult> GetCurrentConditionsAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "forecast/current")] HttpRequest req,
            ILogger log)
        {
            string coordinates = req.Query["coordinates"];

            var currentConditionsHttpResponse = await httpClient.GetAsync(string.Format(currentConditionsEndpoint, coordinates));
            var currentConditions = currentConditionsHttpResponse.Content.ReadAsAsync<dynamic>();

            return new OkObjectResult(currentConditions);
        }

        [FunctionName("daily")]
        public static async Task<IActionResult> GetDailyForecastAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "forecast/daily")] HttpRequest req,
            ILogger log)
        {
            string coordinates = req.Query["coordinates"];

            var httpResponse = await httpClient.GetAsync(string.Format(dailyForecastEndpoint, coordinates));
            var content = httpResponse.Content.ReadAsAsync<dynamic>();

            return new OkObjectResult(content);
        }

        [FunctionName("hourly")]
        public static async Task<IActionResult> GetHourlyForecastAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "forecast/hourly")] HttpRequest req,
            ILogger log)
        {
            string coordinates = req.Query["coordinates"];

            var httpResponse = await httpClient.GetAsync(string.Format(hourlyForecastEndpoint, coordinates));
            var content = httpResponse.Content.ReadAsAsync<dynamic>();

            return new OkObjectResult(content);
        }

        [FunctionName("minuteByMinute")]
        public static async Task<IActionResult> GetMinuteByMinuteForecastAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "forecast/minutebyminute")] HttpRequest req,
            ILogger log)
        {
            string coordinates = req.Query["coordinates"];

            var httpResponse = await httpClient.GetAsync(string.Format(minuteByMibuteForecastEndpoint, coordinates));
            var content = httpResponse.Content.ReadAsAsync<dynamic>();

            return new OkObjectResult(content);
        }

    }
}
