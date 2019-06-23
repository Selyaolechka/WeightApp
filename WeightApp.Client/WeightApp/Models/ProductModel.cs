﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace WeightApp.Client.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class ProductModel
    {
        /// <summary>
        /// Initializes a new instance of the ProductModel class.
        /// </summary>
        public ProductModel() { }

        /// <summary>
        /// Initializes a new instance of the ProductModel class.
        /// </summary>
        public ProductModel(int id, int calories, int carbohydrates, int proteins, int fats, int? userId = default(int?), string name = default(string), ProductCategoryModel productCategory = default(ProductCategoryModel))
        {
            Id = id;
            UserId = userId;
            Name = name;
            ProductCategory = productCategory;
            Calories = calories;
            Carbohydrates = carbohydrates;
            Proteins = proteins;
            Fats = fats;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public int? UserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "productCategory")]
        public ProductCategoryModel ProductCategory { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "calories")]
        public int Calories { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "carbohydrates")]
        public int Carbohydrates { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "proteins")]
        public int Proteins { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fats")]
        public int Fats { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.ProductCategory != null)
            {
                this.ProductCategory.Validate();
            }
        }
    }
}