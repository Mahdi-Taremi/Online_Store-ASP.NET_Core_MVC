using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        public int BasketId { get; set; }
        public string UserId { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
        public List<Product> Products { get; set; }
    }
}
