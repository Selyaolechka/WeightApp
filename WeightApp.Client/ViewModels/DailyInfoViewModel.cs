using System;

using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class DailyInfoViewModel
    {
        public DailyInfoViewModel(DailyMealInstanceModel selectedDailyMealInstance)
        {
            DailyLimit = selectedDailyMealInstance.DailyNorm;
            Consumed = selectedDailyMealInstance.Consumed;
            Date = selectedDailyMealInstance.DateTime;
        }

        public double DailyLimit { get; }

        public double Consumed { get; }

        public DateTime Date { get; }
    }
}