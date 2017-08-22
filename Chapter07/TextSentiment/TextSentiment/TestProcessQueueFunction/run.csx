using System;

public static void Run(TimerInfo myTimer, ICollector<string> outputSbEmailQueue, TraceWriter log)
{
	// Create dummy email message
	string dummyEmail = $"Dummy email string for Service Bus email queue created at: {DateTime.Now}";
	log.Info(dummyEmail);

	// Add the message to Service Bus email queue
	outputSbEmailQueue.Add(dummyEmail);



}