using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Controllers;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class DbContextProject : IdentityDbContext
    {
        public DbContextProject(DbContextOptions<DbContextProject> options) : base(options)
        {
        }
        public DbSet<Product> Product { get; set;}
        public DbSet<Basket> Basket { get; set; }
    }
}
