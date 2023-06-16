using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Online_Store_ASP.NET_Core_MVC.Models
{
    public class DbContextProject : IdentityDbContext
    {
        public DbContextProject(DbContextOptions<DbContextProject> options) : base(options)
        {
        }
    }
}
