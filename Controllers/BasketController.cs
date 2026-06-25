using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;
using Online_Store_ASP.NET_Core_MVC.Services;
using System.Security.Claims;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{

    public class BasketController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DbContextProject _context;
        private readonly IProductRepository _productRepository;
        public BasketController(UserManager<IdentityUser> userManager, DbContextProject context, IProductRepository productRepository)
        {
            _userManager = userManager;
            _context = context;
            _productRepository = productRepository;
        }

        [HttpPost("AddToBasket")]
        [Authorize(Roles = UsersRoles.USER)]
        public async Task<IActionResult> AddToBasket(int id)
        {

            if (_userManager.Users == null)
            {
                return BadRequest("!!!!!!!!!!");
            }
            // Get the currently logged in user
            var user = await _userManager.GetUserAsync(User);
            var UserNamee =  user.UserName;
            var Email =  user.Email;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the product from the database
            var product = await _productRepository.GetByIdAsync(id);
         
            if (product == null)
            {
                return NotFound("Product Not Found ! 404 ");
            }
            var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == id));
            if (product.IdBasket != null)
            {
                basket.Counter++;
                await _context.SaveChangesAsync();

            } else if (product.IdBasket == null)
            {
                Basket basketdb = new Basket();

                basketdb.BasketId = id;
                basketdb.UserId = userId;
                basketdb.Counter = 1;
                _context.Basket.Add(basketdb);
                await _context.SaveChangesAsync();

            }   
              return Ok("Successfull");
        }
    }
}
