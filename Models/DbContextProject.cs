using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Controllers;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class DbContextProject : IdentityDbContext<ApplicationUser>
    {
        public DbContextProject(DbContextOptions<DbContextProject> options) : base(options)
        {
        }
        public DbSet<Models.Product> Product { get; set; } = default!;
        public DbSet<Basket> Basket { get; set; }
        public DbSet<Order> Order { get; set; }

    
        //public DbSet<CartDetail> CartDetail { get; set; }
        //public DbSet<ShoppingCart> ShoppingCart { get; set; }

    }
}
