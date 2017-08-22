using System;
using System.Collections.Generic;

namespace TextScoreDashboard.Models
{
    public partial class AverageDocumentScore
    {
        public int Id { get; set; }
        public int DocumentCount { get; set; }
        public double AverageScore { get; set; }
        public DateTime Date { get; set; }
    }
}
