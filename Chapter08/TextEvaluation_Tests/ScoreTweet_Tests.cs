using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEvaluation;
using Microsoft.Azure.ApiHub;
using Moq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;

namespace TextEvaluation_Tests
{
	[TestClass()]
	public class ScoreTweet_Tests
	{
		[TestMethod()]
		public async Task ScoreTweet_ExistingTweet_UpdatedEntity()
		{
			////////////////////////////////////////////
			// Arrange
			////////////////////////////////////////////
// Setup Logs
var logMessages = new List<string>();

var log = new Mock<TraceWriter>(TraceLevel.Error);
log.Setup(l => l.Trace(It.IsAny<TraceEvent>()))
	.Callback((TraceEvent t) => logMessages.Add(t.Message));

			var tweetText = "Loving the serverless compute :-)";
			var user = "DivineOps";
			var score = 0.949561040962202;
			var expectedLogMessages = new List<string>()
			{
				"Updating existing row in output table" ,
				$"Scored tweet: {tweetText}, user: {user}, sentiment score: {score}"
			};

			// Mock outputTable
			var outputTable = new Mock<ITable<JObject>>();
			var entity = new JObject {
				new JProperty("ID", "0"),
				new JProperty("Username", user),
				new JProperty("TweetText", tweetText),
				new JProperty("TextSentimentScore", score)
			};

			var entities = new SegmentedResult<JObject>();
			entities.Items = new List<JObject>() { entity };
			// Setup the ListEntitiesAsync method 
			// To return a list with a single entity defined above
			outputTable.Setup(t => t.ListEntitiesAsync(It.IsAny<Query>(),
					It.IsAny<ContinuationToken>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(entities);

			// HTTP request
			var req = new HttpRequestMessage()
			{
				Content = new StringContent(
				"{\"TweetText\": \"" + tweetText + "\", " +
				"\"UserDetails\": {\"UserName\": \"" + user + "\"}}")
			};

			////////////////////////////////////////////
			// Act 
			////////////////////////////////////////////
			await ScoreTweet.Run(req, outputTable.Object, log.Object);

			////////////////////////////////////////////
			// Assert
			////////////////////////////////////////////
			// Compare all log messages
			CollectionAssert.AreEqual(expectedLogMessages, logMessages);
			// Verify that Update entity was called
			outputTable.Verify(t => t.UpdateEntityAsync(It.IsAny<string>(),
					It.IsAny<JObject>(), It.IsAny<CancellationToken>()),
					Times.Once());

		}
	}
}
