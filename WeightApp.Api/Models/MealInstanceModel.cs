using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class MealProductModel
    {
        [Required]
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class MealInstanceModel
    {
        public int MealInstanceId { get; set; }

        [Required]
        public MealTypeModel MealType { get; set; }

        [Required]
        public MealProductModel Product { get; set; }
        public int GoalId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class DailyMealInstanceModel
    {
        public DateTime DateTime { get; set; }
        public double DailyNorm { get; set; }
        public double Consumed { get; set; }

        public IEnumerable<MealInstanceModel> Meals { get; set; }
    }
}