using System.Net;
using Microsoft.Azure.ApiHub;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ITable<TextScore> outputTable, TraceWriter log)
{
    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    if (name == null)
    {
        log.Error($"Name parameter missing in HTTP request. RequestUri={req.RequestUri}");
        return req.CreateResponse(HttpStatusCode.BadRequest,
            "Please pass name parameter in the query string");
    }

    // Generate random text score between 0 and 1
    double score = new Random().NextDouble();
    var textScore = new TextScore() { DocumentName = name, TextSentimentScore = score };


    //Store the text score record in the database
    await outputTable.CreateEntityAsync(textScore);
    log.Info($"Randomized text score function returned a score of {score}. RequestUri={req.RequestUri}");

    //Return the text score in HTTP response
    return req.CreateResponse(HttpStatusCode.OK, "The text sentiment score is  " + score);
}


public class TextScore
{
    public string DocumentName { get; set; }
    public double TextSentimentScore { get; set; }
}

