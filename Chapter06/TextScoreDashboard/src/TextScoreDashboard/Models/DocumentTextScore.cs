using System;
using System.Collections.Generic;

namespace TextScoreDashboard.Models
{
    public partial class DocumentTextScore
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public DateTime Date { get; set; }
        public double TextSentimentScore { get; set; }
    }
}
