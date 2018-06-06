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
    public class Function
    {
        private static readonly HttpClient client = new HttpClient();
        private static string UrlToTest = "https://comparethemarket.com";

        private static async Task<string> GetPageSpeedScore()
        {
            var googlePageSpeedApi = "https://www.googleapis.com/pagespeedonline/v4/runPagespeed?";
            var urlSeparator = "url=";
            var keySeparator = "?key=";
            
            // var response = await client.GetAsync(googlePageSpeedApi + urlSeparator + UrlToTest + keySeparator + apiKey).ConfigureAwait(continueOnCapturedContext:false);
            // var json = await response.Content.ReadAsStringAsync();
            // dynamic d = JsonConvert.DeserializeObject(json);
            // return d.ruleGroups.SPEED.score;
            return "foo";
        }

        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {

            string pageSpeedScore = GetPageSpeedScore().Result;
            Dictionary<string, string> body = new Dictionary<string, string>
            {
                { "Site", UrlToTest },
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

    public class PageSpeed
    {
        private static readonly HttpClient client = new HttpClient();

        public APIGatewayProxyResponse PageSpeedHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 200
            };
        }
    }
}
