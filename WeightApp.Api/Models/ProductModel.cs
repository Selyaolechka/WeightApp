namespace WeightApp.Api.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public ProductCategoryModel ProductCategory { get; set; }
        public int Calories { get; set; }
        public int Carbohydrates { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
    }
}