//using System;
//using Microsoft.Azure.WebJobs.Host;
//using System.Diagnostics;
//using System.Collections.Generic;

//namespace TextEvaluation_Tests
//{
//	public class TraceWriterWrapper : TraceWriter
//	{
//		public TraceWriterWrapper(TraceLevel level = TraceLevel.Error) 
//			: base(level)
//		{
//			LogMessages = new List<string>();
//		}

//		public List<String> LogMessages { get; set; }

//		public override void Trace(TraceEvent traceEvent)
//		{
//			LogMessages.Add(traceEvent.Message);
//		}
//	}
//}
