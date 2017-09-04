using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.ApiHub;
using Microsoft.Azure.WebJobs.Host;
using System.Threading;

namespace TextEvaluation
{
	public static class StoreOutput
    {
		public static async Task StoreOutputSQL(JObject output, ITable<JObject> outputTable, TraceWriter log, string nameProperty = null)
		{
			// If updating existing entities is required
			if (nameProperty != null)
			{
				// Get property value
				var entityName = output[nameProperty].Value<string>();
				// Construct comparison query
				var query = "$filter=" + nameProperty + 
          " eq '" + entityName + "'";
				//Get existing entities
				var entities = await outputTable.ListEntitiesAsync(query: Query.Parse(query));

				if (entities.Items.Count() == 1)
				{
					// Update the entry if it exists
					log.Info($"Updating existing row in output table");
					var entityId = entities.Items[0]["ID"].Value<string>();
					await outputTable.UpdateEntityAsync(entityId, output);
				}
				else if (entities.Items.Count() == 0)
				{
					log.Info($"Creating new row in output table");
					await outputTable.CreateEntityAsync(output);
				}
				else
				{
					// More than one existing entity - shouldn't be possible
					// Handle error
				}
			}
			else
			{
				log.Info($"Creating new row in output table");
				await outputTable.CreateEntityAsync(output);
			}
		}
	}
}
