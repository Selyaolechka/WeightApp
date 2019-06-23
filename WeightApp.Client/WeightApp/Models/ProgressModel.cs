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

    public partial class ProgressModel
    {
        /// <summary>
        /// Initializes a new instance of the ProgressModel class.
        /// </summary>
        public ProgressModel() { }

        /// <summary>
        /// Initializes a new instance of the ProgressModel class.
        /// </summary>
        public ProgressModel(int progressId, int goalId, int weight, DateTime date)
        {
            ProgressId = progressId;
            GoalId = goalId;
            Weight = weight;
            Date = date;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "progressId")]
        public int ProgressId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "goalId")]
        public int GoalId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}