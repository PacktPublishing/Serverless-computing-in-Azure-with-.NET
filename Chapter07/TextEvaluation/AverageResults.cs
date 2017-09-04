using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.ApiHub;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace TextEvaluation
{
	public static class AverageResults
	{
		[FunctionName("AverageResults")]
		public static async void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer,
			[ApiHubTable(connection: "azureSQL", TableName = "DocumentTextScore")]ITable<TextScore> scoresTable,
			[ApiHubTable(connection: "azureSQL", TableName = "AverageDocumentScore")]ITable<JObject> averageTable,
			TraceWriter log)
		{
			log.Info($"Average score function executed at: {DateTime.Now}");
			// Calculate the average score for all existing documents
			var scores = await scoresTable.ListEntitiesAsync();
			var average = scores.Items.Average(p => p.TextSentimentScore);
			var count = scores.Items.Count();

			// Create new score entity as JObject
			var scoreAverage = new JObject {
					new JProperty("DocumentCount", count),
					new JProperty("AverageScore", average)
				 };

			// Get the average score record for today's date from AverageDocumentScore SQL Table
			var today = XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc);
			var query = "$filter=Date eq Date(" + today + ")";
			var entities = await averageTable.ListEntitiesAsync(query: Query.Parse(query));

			if (entities.Items.Count() == 1)
			{
				// Update the entry if it exists
				log.Info($"Updating average sentiment score for today to average={average}");
				var entityId = entities.Items[0]["ID"].Value<string>();
				await averageTable.UpdateEntityAsync(entityId, scoreAverage);
			}
			else
			{
				// Create new entry
				log.Info($"Creating new average sentiment score for today average={average}");
				await averageTable.CreateEntityAsync(scoreAverage);
			}
		}
	}
}