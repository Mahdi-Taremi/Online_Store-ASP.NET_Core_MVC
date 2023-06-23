using Online_Store_ASP.NET_Core_MVC;

namespace Online_Store_ASP.NET_Core_MVC.Entity
{
    public class ProductVariantEntity
    {
        public int Id { get; set; }
        public string? productId { get; set; } = null!;
        public int? Price { get; set; }
        public int? ColorId { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
