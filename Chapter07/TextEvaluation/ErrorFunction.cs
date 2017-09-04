using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TextEvaluation
{
    public static class ErrorFunction
    {
        [FunctionName("ErrorFunction")]
        public static void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, TraceWriter log)
        {
			//// Throw unhandled exception
			//throw new Exception("This is a unhandled exception",
			//new Exception("This is an inner exception"));

			try
			{
				// Throw a handled exception
				throw new Exception("This is a handled exception",
				new Exception("This is an inner exception"));
			}
			catch (Exception ex)
			{
				log.Error($"Caught an exception: {ex.ToString()}");
			}
		}
    }
}