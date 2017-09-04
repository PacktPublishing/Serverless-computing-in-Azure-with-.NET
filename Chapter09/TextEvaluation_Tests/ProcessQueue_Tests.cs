using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEvaluation;
using Microsoft.Azure.WebJobs.Host;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace TextEvaluation_Tests
{
[TestClass()]
public class ProcessQueue_Tests
{
[TestMethod()]
public void ProcessQueueRun_InputString_LoggedInput()
{
	////////////////////////////////////////////
	// Arrange
	////////////////////////////////////////////
	var emailBody = "Test Service Bus message";
	var expectedLogMessage = $"{{'messageContent': {emailBody}}}";
	var logMessages = new List<string>();
	// set up the log mock
	var log = new Moq.Mock<TraceWriter>(TraceLevel.Error);
	log.Setup(l => l.Trace(It.IsAny<TraceEvent>()))
		.Callback((TraceEvent t) => logMessages.Add(t.Message));

	////////////////////////////////////////////	
	// Act 
	////////////////////////////////////////////

	ProcessQueue.Run(emailBody, log.Object);
	////////////////////////////////////////////
	// Assert
	////////////////////////////////////////////
	Assert.AreEqual(expectedLogMessage, logMessages.First());
}
}
}
