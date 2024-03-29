﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace WeightApp.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Extension methods for ReferenceData.
    /// </summary>
    public static partial class ReferenceDataExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<MealTypeModel> GetMealTypes(this IReferenceData operations)
            {
                return Task.Factory.StartNew(s => ((IReferenceData)s).GetMealTypesAsync(), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<MealTypeModel>> GetMealTypesAsync(this IReferenceData operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetMealTypesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<ProductCategoryModel> GetProductCategories(this IReferenceData operations)
            {
                return Task.Factory.StartNew(s => ((IReferenceData)s).GetProductCategoriesAsync(), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ProductCategoryModel>> GetProductCategoriesAsync(this IReferenceData operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetProductCategoriesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
