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

    public partial class TrackProgressRequest
    {
        /// <summary>
        /// Initializes a new instance of the TrackProgressRequest class.
        /// </summary>
        public TrackProgressRequest() { }

        /// <summary>
        /// Initializes a new instance of the TrackProgressRequest class.
        /// </summary>
        public TrackProgressRequest(int goalId, int weight, DateTime date)
        {
            GoalId = goalId;
            Weight = weight;
            Date = date;
        }

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
