using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class DbContextProject : IdentityDbContext
    {
        public DbContextProject(DbContextOptions<DbContextProject> options) : base(options)
        {
        }
        public DbSet<Models.Product> Product { get; set; } = default!;
        public DbSet<Basket> Basket { get; set; }
        public DbSet<CartDetail> CartDetail { get; set; }
        //public DbSet<ShoppingCart> ShoppingCart { get; set; }

    }
}
