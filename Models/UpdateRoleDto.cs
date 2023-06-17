using System.ComponentModel.DataAnnotations;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
    }
}
