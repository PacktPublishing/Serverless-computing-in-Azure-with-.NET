using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using Microsoft.Azure.ApiHub;
using Microsoft.Extensions.Logging;

namespace TextEvaluation
{
	public static class ScoreText
	{
		[FunctionName("ScoreText")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
			[ApiHubTable(connection: "azureSQL", TableName = "DocumentTextScore")]ITable<TextScore> outputTable,
			Microsoft.Extensions.Logging.ILogger log, 
			ExecutionContext executionContext)
		{
			// parse query parameter
			string name = req.GetQueryNameValuePairs()
					.FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
					.Value;
			if (name == null)
			{
				log.LogError($"Name parameter missing in HTTP request. RequestUri={req.RequestUri}");
				return req.CreateResponse(HttpStatusCode.BadRequest,
				   "Please pass name parameter the in query string");
			}

			// Generate random text score between 0 and 1
			var score = new Random().NextDouble();
			var textScore = new TextScore() { DocumentName = name, TextSentimentScore = score };

			//Store the text score record in the database
			await outputTable.CreateEntityAsync(textScore);
			log.LogInformation($"1.0 Randomized text score function returned a score of {score}. RequestUri={req.RequestUri}, " +
				$"invocation ID={executionContext.InvocationId}");

			//Return the text score in HTTP response
			return req.CreateResponse(HttpStatusCode.OK, "The text sentiment score is  " + score);
		}
	}
}