using System;

namespace WeightApp.Db
{
    public class MealInstanceEntity
    {
        public int MealInstanceId { get; set; }
        public int Amount { get; set; }
        public GoalEntity Goal { get; set; }
        public DateTime Date { get; set; }
        public ProductEntityRef Product { get; set; }
        public MealTypeEntity MealType { get; set; }
    }
}