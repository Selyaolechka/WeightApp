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

    public partial class AddProductRequest
    {
        /// <summary>
        /// Initializes a new instance of the AddProductRequest class.
        /// </summary>
        public AddProductRequest() { }

        /// <summary>
        /// Initializes a new instance of the AddProductRequest class.
        /// </summary>
        public AddProductRequest(string name, int categoryId, int calories, int carbohydrates, int proteins, int fats)
        {
            Name = name;
            CategoryId = categoryId;
            Calories = calories;
            Carbohydrates = carbohydrates;
            Proteins = proteins;
            Fats = fats;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "categoryId")]
        public int CategoryId { get; set; }

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
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (this.Name != null)
            {
                if (this.Name.Length < 1)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Name", 1);
                }
            }
        }
    }
}