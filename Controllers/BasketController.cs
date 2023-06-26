using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Online_Store_ASP.NET_Core_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
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
                return NotFound("Product Not Found ! 404 ");
            }


            // Basket basketdb = new Basket();
            // Get the user's basket from the database or create a new one if it doesn't exist



            var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == id));


           // var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == 4));
            //var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == 4));

            //var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == product.IdBasket));
            //var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == product.IdBasket));

            //var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id && b.BasketId == product.IdBasket);





            //var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId.ToString() == (product.IdBasket).ToString()));
            //var basket = await _context.Basket.FirstOrDefaultAsync(b => (b.UserId == user.Id) && (b.BasketId == product.IdBasket));
            //var basket = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id);
            //var basket = _context.Basket.Where(x => x.UserId == user.Id).SingleOrDefault();
            //var basket = _context.Basket.Where(x => x.UserId == user.Id && x.BasketId == int.Parse(product.IdBasket)).SingleOrDefault();
            //var basket = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id);

            //var basket = _context.Basket.SingleOrDefault(x => x.UserId == user.Id);
            //var basket = await _context.Basket.SingleOrDefaultAsync(b => b.UserId == user.Id);
            //var basket = await _context.Basket.Where(b => b.UserId == user.Id).SingleOrDefaultAsync();
            //var basket =  _context.Basket.Where(b => b.UserId == user.Id).SingleOrDefault();
            //var basket = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id);

            //var basket = _context.Basket.SingleOrDefault(x => x.UserId == user.Id);
            //var basket = _context.Basket.SingleOrDefault(x => x.UserId == product.IdBasket);
            //var basket = await _context.Basket.SingleOrDefaultAsync(b => b.UserId == user.Id);
            if (product.IdBasket != null)
            {
                basket.Counter++;
                await _context.SaveChangesAsync();

            } else if (product.IdBasket == null)
            {
               // Basket basketdb = new Basket();
                Basket basketdb = new Basket();
                //basketdb.Products = new List<Product>();
                /* Test DB => basketdb.BasketId = id;
                 basketdb.Counter = 1; */
                basketdb.BasketId = id;
                basketdb.UserId = userId;
                basketdb.Counter = 1;
                _context.Basket.Add(basketdb);
                //basketdb.Products.Add(product);
                await _context.SaveChangesAsync();

            }
            
            /*if (basket == null)
            {
                //basket = new Basket { UserId = user.Id };
                basket = new Basket { UserId = user.Id, Products = new List<Product>() };

                _context.Basket.Add(basket);
            }*/
            // Add the product to the basket
            // basket.Products.Add(product);
           
              return Ok("Successfull");
        }
    }
}

