using System;
using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsMale { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}