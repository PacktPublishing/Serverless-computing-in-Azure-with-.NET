using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using HtmlAgilityPack;

namespace TextEvaluation
{
	public static class ProcessQueue
	{
		[FunctionName("ProcessQueue")]
		public static void Run(
			[ServiceBusTrigger("emailqueue", AccessRights.Listen, Connection = "serviceBus")]string emailBody, 
			TraceWriter log)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(emailBody);
			var text = doc.DocumentNode.InnerText;
			log.Info($"C# ServiceBus queue trigger function processed message: {text}");
		}
	}
}