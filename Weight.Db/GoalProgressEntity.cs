using System;

namespace WeightApp.Db
{
    public class GoalProgressEntity
    {
        public int ProgressId { get; set; }
        public int GoalId { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
    }
}