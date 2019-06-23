using System;

namespace WeightApp.Api.Models
{
    public class UpdateUserRequest
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}