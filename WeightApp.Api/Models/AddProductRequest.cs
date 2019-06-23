using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class AddProductRequest
    {
        [Required]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Calories { get; set; }
        public int Carbohydrates { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
    }
}