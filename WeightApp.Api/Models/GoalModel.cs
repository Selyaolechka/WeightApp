using System;

namespace WeightApp.Api.Models
{
    public class GoalModel
    {
        public int GoalId { get; set; }
        public DateTime StartDate { get; set; }
        public int Weight { get; set; }
        public int WeightGoal { get; set; }
        public int Height { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSuccess { get; set; }
    }
}