using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

public static async Task Run(string emailBody, TraceWriter log)
{
    var doc = new HtmlDocument();
    doc.LoadHtml(emailBody);
    var text = doc.DocumentNode.InnerText;
    log.Info($"C# ServiceBus queue trigger function processed message: {text}");
}