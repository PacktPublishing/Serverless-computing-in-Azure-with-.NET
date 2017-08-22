#load "models.csx"

using System;
using Microsoft.Azure.ApiHub;

public static void Run(Stream myBlob, string name, ITable<TextScore> outputTable, TraceWriter log)
{
    var blobContent = new StreamReader(myBlob).ReadToEnd();
    
    // Generate random text score between 0 and 1
    double score = new Random().NextDouble();
    var textScore = new TextScore() { DocumentName = name, TextSentimentScore = score };

    //Store the text score record in the database
    outputTable.CreateEntityAsync(textScore);
    log.Info($"Randomized text score function processed a blob file {name} and returned a score of {score}");

}