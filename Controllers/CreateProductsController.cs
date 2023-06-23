using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    public class CreateProductsController : Controller
    {
        private readonly DbContextProject _context;
        public CreateProductsController(DbContextProject context)
        {
            _context = context;
        }

        // GET: CreateProductsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateProductsController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //try
        //{
        //    return RedirectToAction(nameof(Index));
        // }
        // catch
        // {
        //  return View();
        // }
        // }
        [HttpPost("CreateProduct")]
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

            //await _context.SaveChangesAsync();
            return Ok("Add Product");
            //return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // GET: CreateProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
