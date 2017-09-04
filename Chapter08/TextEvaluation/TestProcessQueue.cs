using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace TextEvaluation
{
    public static class TestProcessQueue
    {
		[FunctionName("TestProcessQueue")]
		public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer,
			[ServiceBus("emailqueue", AccessRights.Send, Connection = "serviceBusSend")]ICollector<string> outputSbEmailQueue,
			TraceWriter log)
		{
			// Create dummy email message
			string dummyEmail = $"Dummy email string for Service Bus email queue created at: {DateTime.Now}";
			log.Info(dummyEmail);
			// Add the message to Service Bus email queue
			outputSbEmailQueue.Add(dummyEmail);
		}
	}
}