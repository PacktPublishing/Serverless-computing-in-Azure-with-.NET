#r "Newtonsoft.Json"
#load "..\Shared\ScoreText.csx"
#load "..\Shared\StoreOutput.csx"

using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.ApiHub;

public static async Task Run(HttpRequestMessage req, ITable<JObject> outputTable, TraceWriter log)
{
    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);
    var tweetText = data.TweetText.ToString();
    var user = data.UserDetails.UserName.ToString();

    //Call shared code to score the tweet text
    var score = await ScoreText.ScoreTextSentiment(tweetText);
	// Create new score entity as JObject
	var tweetScore = new JObject {
			new JProperty("Username", user),
			new JProperty("TweetText", tweetText),
			new JProperty("TextSentimentScore", score)
		 };

	//Store the text score record in the database
	await StoreOutput.StoreOutputSQL(tweetScore, outputTable, log, "TweetText");
	log.Info($"Scored tweet: {tweetText}, user: {user}, sentiment score: {score}");
}

