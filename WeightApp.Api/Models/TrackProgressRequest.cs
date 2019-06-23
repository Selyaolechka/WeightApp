using System;

namespace WeightApp.Api.Models
{
    public class TrackProgressRequest
    {
        public int GoalId { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
    }
}