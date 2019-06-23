using System;

using Dapper.Contrib.Extensions;

namespace WeightApp.Db
{
    public class GoalEntity
    {
        public int GoalId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public int Weight { get; set; }
        public int WeightGoal { get; set; }
        public int Height { get; set; }
        public bool IsCompleted { get; set; }

        [Write(false)]
        [Computed]
        public bool IsSuccess { get; set; }
    }
}