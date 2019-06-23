using System;

namespace WeightApp.Api.Services
{
    public class WeightCalculator : IWeightCalculator
    {
        public double CalculateDailyCaloriesNorm(
            int weight, 
            int height, 
            DateTime birthDate, 
            DateTime startDate,
            bool isMale)
        {
            var age = startDate.Year - birthDate.Year;

            if (isMale)
            {
                return 5 + (10 * weight) + (6.25 * height) - (5 - age);
            }

            return (10 * weight) + (6.25 * height) - (5 - age);
        }
    }
}