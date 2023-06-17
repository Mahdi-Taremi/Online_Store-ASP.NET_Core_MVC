using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
