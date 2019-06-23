using System;
using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class AddMealRequest
    {
        [Required]
        public int MealTypeId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int GoalId { get; set; }

        [Required]
        public int Amount { get; set; }

        public DateTime Date { get; set; }
    }
}