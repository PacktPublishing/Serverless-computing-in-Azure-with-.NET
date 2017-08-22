#r "Newtonsoft.Json"
#load "..\Shared\ScoreText.csx"
#load "..\Shared\StoreOutput.csx"


using System;
using Microsoft.Azure.ApiHub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task Run(Stream myBlob, string name, ITable<JObject> outputTable, TraceWriter log)
{
    var blobContent = new StreamReader(myBlob).ReadToEnd();
    
    //Call shared code to score the tweet text
	var textScore = await ScoreText.ScoreTextSentiment(blobContent);

	// Create new score entity as JObject
	var documentScore = new JObject {
			new JProperty("DocumentName", name),
			new JProperty("TextSentimentScore", textScore)
		 };

	//Store the text score record in the database
	await StoreOutput.StoreOutputSQL(documentScore, outputTable, log, "DocumentName");
	log.Info($"Document scoring function processed a text file \"{name}\" and returned a score of {textScore}");

}