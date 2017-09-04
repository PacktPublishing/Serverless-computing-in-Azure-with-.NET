using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.ApiHub;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TextEvaluation
{
	public static class ScoreDocument
	{
		[FunctionName("ScoreDocument")]
		public static async Task Run(
			[BlobTrigger("documents/{name}", Connection = "blobStorageConnection")]Stream myBlob,
			[ApiHubTable(connection: "azureSQL", TableName = "DocumentTextScore")]ITable<JObject> outputTable,
			string name, TraceWriter log)
		{
			var blobContent = new StreamReader(myBlob).ReadToEnd();

			//Call shared code to score the tweet text
			var textScore = await new EvaluateText().ScoreTextSentiment(blobContent);

			// Create new score entity as JObject
			var documentScore = new JObject {
				new JProperty("DocumentName", name),
				new JProperty("TextSentimentScore", textScore)
			 };

			//Store the text score record in the database
			await StoreOutput.StoreOutputSQL(documentScore, outputTable, log, "DocumentName");
			log.Info($"Document scoring function processed a text file \"{name}\" and returned a score of {textScore}");
		}
	}
}