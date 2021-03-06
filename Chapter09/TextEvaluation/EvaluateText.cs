﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEvaluation
{
public class EvaluateText
{
private readonly HttpClient _client;

public EvaluateText(HttpMessageHandler messageHandler = null)
{
	if (messageHandler == null)
	{
		_client = new HttpClient();
	}
	else
	{
		_client = new HttpClient(messageHandler);
	}
}

		public async Task<double> ScoreTextSentiment(string inputText)
		{

			// Generate random text score between 0 and 1
			var score = await CallTextSentimentApi(inputText);

			return score;
		}

		public async Task<double> CallTextSentimentApi(string inputText)
		{

			// Get the API URL and the API key from settings
			var uri = ConfigurationManager.AppSettings["textSentimentApiUrl"];
			var apiKey = ConfigurationManager.AppSettings["textSentimentApiKey"];

			// Configure the HttpClient request headers
			_client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			// Create the JSON payload from the input text
			var jsonPayload = ConstructJsonPayload(inputText);

			byte[] byteData = Encoding.UTF8.GetBytes(jsonPayload);
			var content = new ByteArrayContent(byteData);
			// Add application/json header for the content
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Call the Text Sentiment API to get the scores
			var response = await _client.PostAsync(uri, content);
			var jsonResponse = await response.Content.ReadAsStringAsync();

			//Calculate the average score
			var overallScore = CalculateScore(jsonResponse);
			return overallScore;

		}

		public string ConstructJsonPayload(string inputText)
		{
			// Remove new line characters
			inputText = inputText.Replace(Environment.NewLine, String.Empty);
			// Split the input text into sentences
			var sentences = inputText.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

			// Create the list of documents, and add sentences to the list
			var list = new List<document>();
			// Limit the document list at 1000, Text Sentiment API limitation. 
			// Note: when working with large documents, a different text split procedure should be defined
			for (int i = 0; i < sentences.Count() && i < 1000; i++)
			{
				list.Add(new document
				{
					id = i.ToString(),
					text = sentences[i]
				});
			}
			// Create the payload class
			var docs = new documentList() { documents = list };
			// Serialize the payload to JSON
			var json = JsonConvert.SerializeObject(docs);
			return json;
		}

		public static double CalculateScore(string jsonResponse)
		{
			// Deserialize the response from JSON
			var documentScoreList = JsonConvert.DeserializeObject<documentList>(jsonResponse);
			// Calculate and return the average text score across different sentences
			var averageTextScore = documentScoreList.documents.Average(p => p.score);
			return averageTextScore;
		}

	}
}
