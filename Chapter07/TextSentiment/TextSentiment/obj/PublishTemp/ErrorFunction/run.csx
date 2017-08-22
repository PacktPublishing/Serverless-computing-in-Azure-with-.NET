using System;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
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