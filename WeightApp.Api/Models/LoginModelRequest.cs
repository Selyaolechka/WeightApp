using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class LoginModelRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}