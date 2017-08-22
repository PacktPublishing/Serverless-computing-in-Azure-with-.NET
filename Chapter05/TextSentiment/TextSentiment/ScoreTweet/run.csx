#r "Newtonsoft.Json"
#load "..\Shared\ScoreText.csx"

using System;
using System.Net;
using Newtonsoft.Json;

public static async void Run(HttpRequestMessage req, TraceWriter log)
{
    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);
    var tweetText = data.TweetText.ToString();
    var user = data.UserDetails.UserName.ToString();

    //Call shared code to score the tweet text
    var score = ScoreText.ScoreTextSentiment(tweetText);

    log.Info($"Scored tweet: {tweetText}, user: {user}, sentiment score: {score}");

}

