using System;
using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class SetNewGoalRequest
    {
        public int Weight { get; set; }
        public int WeightGoal { get; set; }
        public int Height { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
    }
}