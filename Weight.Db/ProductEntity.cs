namespace WeightApp.Db
{
    public class ProductEntityRef
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    public class ProductEntity : ProductEntityRef
    {
        public int? UserId { get; set; }
        public ProductCategoryEntity ProductCategory { get; set; }
        public int Calories { get; set; }
        public int Carbohydrates { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
    }
}