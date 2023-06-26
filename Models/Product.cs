using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Price")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Please Enter Quantity")]
        public int Quantity { get; set; }
        public string? Color { get; set; }
        public string? pic_1 { get; set; }
        [NotMapped]
        public IFormFile? UploadFile { get; set; }
        public string? Description { get; set; }
        [ForeignKey("BasketId")]
        public ICollection<Basket> IdBasket { get; set; }
    }
}
