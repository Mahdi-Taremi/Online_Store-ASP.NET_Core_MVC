namespace Online_Store_ASP.NET_Core_MVC.Entity
{
    public class ProductEntity
    {
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? productImage { get; set; }
        public List<ProductVariantEntity>? Variants { get; set; }
    }
}
