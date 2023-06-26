using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;
using System.Data;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    
    public class ShopController : Controller
    {
        private readonly DbContextProject _context;
        public ShopController(DbContextProject context)
        {
            _context = context;
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = UsersRoles.ADMIN)]
        public IActionResult CreateProduct(Models.Product f, [FromServices] IWebHostEnvironment env)
        {
            if (_context.Product == null)
            {
                return Problem("DbContextProject.Product is null.");
            }
            var FilePath = Path.Combine(env.WebRootPath, "Uploads", f.UploadFile.FileName);
            using (var img = System.IO.File.Create(FilePath))
            {
                f.UploadFile.CopyTo(img);
            }
            f.pic_1 = f.UploadFile.FileName;
            _context.Product.Add(f);
            _context.SaveChanges();

            return Ok("Add Product");
        }


    
        //[ValidateAntiForgeryToken]
        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = UsersRoles.ADMIN)]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var query = _context.Product.Where(x => x.Id == id).SingleOrDefault();
            _context.Product.Remove(query);
            _context.SaveChanges();
            return Ok("Delete Product");

        }

        // Test
        //[ValidateAntiForgeryToken]
        /*[HttpPost("BuyProduct")]
        [Authorize(Roles = UsersRoles.USER)]
        public string BuyProduct(int id)
        {
            var query = _context.Product.Where(x => x.Id == id).SingleOrDefault();
            if (query == null)
            {
                return "UnSuccessfull Add To Basket";
            }
            else
            {
                Basket basketdb = new Basket();
                basketdb.BasketId = id;
                _context.Basket.Add(basketdb);
                _context.SaveChanges();
                var pCount = basketdb.BasketId;
                var bCount = _context.Basket.Count();
                return "Successfull Add product to Basket" + " " + pCount.ToString() + " " + "and Count Basket" + " " + bCount.ToString();
            }
        }*/

        [HttpGet("Show")]
        [Authorize(Roles = UsersRoles.ADMIN)]
        public string Show(int Id)
        {
            var query = _context.Product.Where(x => x.Id == Id).SingleOrDefault();

            return ("Product Name : " + query.Name + " " +  " and Price : " + query.Price);
        }
    }
}
