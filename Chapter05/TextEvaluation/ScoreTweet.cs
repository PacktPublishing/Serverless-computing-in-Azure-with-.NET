using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TextEvaluation
{
	public static class ScoreTweet
	{
		[FunctionName("ScoreTweet")]
		public static async Task Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
		{
			string jsonContent = await req.Content.ReadAsStringAsync();
			dynamic data = JsonConvert.DeserializeObject(jsonContent);
			var tweetText = data.TweetText.ToString();

			var user = data.UserDetails.UserName.ToString();
			//Call shared code to score the tweet text
			var score = new EvaluateText().ScoreTextSentiment(tweetText);
			log.Info($"Scored tweet: {tweetText}, user: {user}, sentiment score:{ score}");
		}
	}
}