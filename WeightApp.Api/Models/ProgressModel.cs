using System;

namespace WeightApp.Api.Models
{
    public class ProgressModel
    {
        public int ProgressId { get; set; }
        public int GoalId { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
    }
}