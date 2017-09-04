using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.ApiHub;
using System;

namespace TextEvaluation
{
    public static class ScoreDocument
    {
        [FunctionName("ScoreDocument")]        
        public static void Run(
			[BlobTrigger("documents/{name}", Connection = "blobStorageConnection")]Stream myBlob,
			[ApiHubTable(connection: "azureSQL", TableName = "DocumentTextScore")]ITable<TextScore> outputTable,
			string name, TraceWriter log)
        {
			var blobContent = new StreamReader(myBlob).ReadToEnd();
			// Generate random text score between 0 and 1
			double score = new Random().NextDouble();
			var textScore = new TextScore() { DocumentName = name, TextSentimentScore = score };
			//Store the text score record in the database
			outputTable.CreateEntityAsync(textScore);
			log.Info($"Randomized text score function processed a blob file {name} and returned a score of {score}");
		}
	}
}