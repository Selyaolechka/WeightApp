using System;

namespace WeightApp.Api.Services
{
    public interface IWeightCalculator
    {
        double CalculateDailyCaloriesNorm(
            int weight,
            int height,
            DateTime birthDate,
            DateTime startDate,
            bool isMale);
    }
}