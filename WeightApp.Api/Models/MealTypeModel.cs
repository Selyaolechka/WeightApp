using System.ComponentModel.DataAnnotations;

namespace WeightApp.Api.Models
{
    public class MealTypeModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}