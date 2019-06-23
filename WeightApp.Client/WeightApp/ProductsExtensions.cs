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
    /// Extension methods for Products.
    /// </summary>
    public static partial class ProductsExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<ProductModel> GetProducts(this IProducts operations)
            {
                return Task.Factory.StartNew(s => ((IProducts)s).GetProductsAsync(), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ProductModel>> GetProductsAsync(this IProducts operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetProductsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='product'>
            /// </param>
            public static ProductModel AddProduct(this IProducts operations, AddProductRequest product)
            {
                return Task.Factory.StartNew(s => ((IProducts)s).AddProductAsync(product), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='product'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProductModel> AddProductAsync(this IProducts operations, AddProductRequest product, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddProductWithHttpMessagesAsync(product, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='productId'>
            /// </param>
            public static void RemoveProduct(this IProducts operations, int productId)
            {
                Task.Factory.StartNew(s => ((IProducts)s).RemoveProductAsync(productId), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='productId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task RemoveProductAsync(this IProducts operations, int productId, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.RemoveProductWithHttpMessagesAsync(productId, null, cancellationToken).ConfigureAwait(false);
            }

    }
}
