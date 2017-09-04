using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TextEvaluation;

namespace TextEvaluation_Tests
{
	[TestClass()]
	public class EvaluateText_Tests
	{
[TestMethod()]
public async Task EvaluateText_ApiReturnsScore_ReturnsTheScore()
{
			////////////////////////////////////////////
			// Arrange
			////////////////////////////////////////////
			const double EPSILON = 0.0000000001;
var expectedScore = 0.500000000000001;
var inputText = "dummy text";
var messageHandler = new Mock<HttpMessageHandler>();
messageHandler.Protected()
	.Setup<Task<HttpResponseMessage>>("SendAsync",
	ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
	.Returns(Task<HttpResponseMessage>.Factory.StartNew(() =>
	{
		return new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(
			"{\"documents\":[{ \"score\": " + expectedScore + "," +
			"\"id\":\"0\" }], \"errors\":[]}")
		};
	}));

////////////////////////////////////////////	
// Act 
////////////////////////////////////////////
var evalText = new EvaluateText(messageHandler.Object);
var score = await evalText.ScoreTextSentiment(inputText);

////////////////////////////////////////////
// Assert
////////////////////////////////////////////
Assert.AreEqual(expectedScore, score, EPSILON);
}

[TestMethod()]
[ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
public async Task EvaluateText_ApiReturnsError_ExceptionThrown()
{
	////////////////////////////////////////////
	// Arrange
	////////////////////////////////////////////
	var inputText = "dummy text";
	var messageHandler = new Mock<HttpMessageHandler>();
	messageHandler.Protected()
		.Setup<Task<HttpResponseMessage>>("SendAsync",
		ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
		.Returns(Task<HttpResponseMessage>.Factory.StartNew(() =>
		{
			return new HttpResponseMessage(HttpStatusCode.InternalServerError);

		}));

	////////////////////////////////////////////	
	// Act 
	////////////////////////////////////////////
	var evalText = new EvaluateText(messageHandler.Object);
	var score = await evalText.ScoreTextSentiment(inputText);

	////////////////////////////////////////////
	// Assert
	////////////////////////////////////////////
	//Expects exception
}
	}
}
