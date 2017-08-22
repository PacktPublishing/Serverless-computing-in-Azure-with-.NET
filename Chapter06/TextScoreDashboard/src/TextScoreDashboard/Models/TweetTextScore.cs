using System;
using System.Collections.Generic;

namespace TextScoreDashboard.Models
{
    public partial class TweetTextScore
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string TweetText { get; set; }
        public double TextSentimentScore { get; set; }
    }
}
