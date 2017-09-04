using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.ApiHub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TextEvaluation
{
	public static class ScoreTweet
	{
		[FunctionName("ScoreTweet")]
		public static async Task Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req,
			[ApiHubTable(connection: "azureSQL", TableName = "TweetTextScore")]ITable<JObject> outputTable,
			TraceWriter log)
		{
			string jsonContent = await req.Content.ReadAsStringAsync();
			dynamic data = JsonConvert.DeserializeObject(jsonContent);
			var tweetText = data.TweetText.ToString();
			var user = data.UserDetails.UserName.ToString();

			//Call shared code to score the tweet text
			var score = await new EvaluateText().ScoreTextSentiment(tweetText);
			// Create new score entity as JObject
			var tweetScore = new JObject {
				new JProperty("Username", user),
				new JProperty("TweetText", tweetText),
				new JProperty("TextSentimentScore", score)
			 };

			//Store the text score record in the database
			await StoreOutput.StoreOutputSQL(tweetScore, outputTable, log, "TweetText");
			log.Info($"Scored tweet: {tweetText}, " +
        $"user: {user}, sentiment score: {score}");

		}
	}
}