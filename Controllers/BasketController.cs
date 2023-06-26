using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;
using System.Data;
using System.Security.Claims;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    /*public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }*/

    public class BasketController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DbContextProject _context;
        //private readonly IBasketService _basketService;

        //public BasketController(UserManager<IdentityUser> userManager, DbContextProject context, BasketService basketService)
        public BasketController(UserManager<IdentityUser> userManager, DbContextProject context)
        {
            _userManager = userManager;
            _context = context;
            //_basketService = basketService;
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
            // var user = _userManager.Where(x => x.Id == User).SingleOrDefault();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Start Test
            /*var basket = await _basketService.GetBasketAsync(userId);
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                _context.Basket.Add(basket);
                await _context.SaveChangesAsync();
            }

            await _basketService.AddProductToBasketAsync(product, basket);

            return Ok("Successfull");*/
            // End Test

            // Get the product from the database
             var product = await _context.Product.FindAsync(id);
            //var product = _context.Product.Where(x => x.Id == id).SingleOrDefault();

           
            if (product == null)
            {
                return NotFound();
            }

           // Basket basketdb = new Basket();


            // Get the user's basket from the database or create a new one if it doesn't exist
             var basket = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id);
            if (basket.Products == null)
            {
                basket.Products = new List<Product>();
            } 
            /*if (basket == null)
            {
                //basket = new Basket { UserId = user.Id };
                basket = new Basket { UserId = user.Id, Products = new List<Product>() };

                _context.Basket.Add(basket);
            }*/

            // Add the product to the basket
            // basket.Products.Add(product);




            basket.Products.Add(product);

              await _context.SaveChangesAsync();




              await _context.SaveChangesAsync();

              return Ok("Successfull");
        }
    }





}

