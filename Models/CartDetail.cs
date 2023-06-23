using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    //[Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required] 
        public int CartDetailId { get; set; }
        [Required]  
        public int PrductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Product Product { get; set; }
        //public ShoppingCart ShoppingCart { get; set; }
       // public int ShoppingCartId { get; set; }
    }
}
