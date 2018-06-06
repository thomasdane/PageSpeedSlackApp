using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HelloWorld
{
    public class PageSpeed
    {
        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> GetPageSpeedScore(string urlToTest)
        {
            var googlePageSpeedApi = "https://www.googleapis.com/pagespeedonline/v4/runPagespeed?";
            var urlSeparator = "url=";
            var keySeparator = "?key=";
            var apiKey = Environment.GetEnvironmentVariable("GOOGLE_PAGE_SPEED_API_KEY");

            var response = await client.GetAsync(googlePageSpeedApi + urlSeparator + urlToTest + keySeparator + apiKey).ConfigureAwait(continueOnCapturedContext:false);
            var json = await response.Content.ReadAsStringAsync();
            dynamic d = JsonConvert.DeserializeObject(json);
            return d.ruleGroups.SPEED.score;
        }

        public APIGatewayProxyResponse PageSpeedHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var url = apigProxyEvent.Body.ToString();
            
            string pageSpeedScore = GetPageSpeedScore(url).Result;
            Dictionary<string, string> body = new Dictionary<string, string>
            {
                { "Site", url },
                { "Google Page Speed", pageSpeedScore },
            };


               return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(body),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
