using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DbContextProject _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(DbContextProject context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        //{
         // if (_context.Product == null)
         // {
            //  return NotFound();
         // }
           //  return await _context.Product.ToListAsync();
        //}

        // GET: api/Products
      //  [HttpGet("GetProduct")]
      //  public async Task<List<Product>> GetProduct()
      //  {
            //var productlist = await this._context.GetProduct();
           // if (productlist != null && productlist.Count > 0)
          //  {
           //     productlist.ForEach(item =>
            //    {
            //        item.productImage = GetImagebyProduct(item.Code);
             //   });
           // }
         //   else
         //   {
         //       return new List<Product>();
          //  }
           // return productlist;
        //}

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Product == null)
          {
              return NotFound();
          }
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /*[HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage()
        {
            bool Result = false;
            try
            {
                var _uploadFile = Request.Form.Files;
                //var file = Request.Form.Files;
                foreach(IFormFile source in _uploadFile)
                {
                    string FileName = source.Name;
                    string FilePath = GetFilePath(FileName);

                    if(!System.IO.File.Exists(FilePath)) {
                    System.IO.Directory.CreateDirectory(FilePath);
                    }
                    string imagePath = FilePath + "\\image.png";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await source.CopyToAsync(stream);
                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Ok(Result);  

        }*/

        /*[NonAction]
        private string GetFilePath(string ProductCode)
        {
            return _webHostEnvironment.WebRootPath + "\\Uploads\\Product" + ProductCode;
        }
        [NonAction]
        private string GetImageProduct(string ProductCode)
        {
            string ImageURL = string.Empty;
            string HostURL = "https://localhost:7245";
            string FilePath = GetFilePath(ProductCode);
            string ImagePath = FilePath +"\\image.png";

            if(!System.IO.File.Exists (ImagePath))
            {
                ImagePath = HostURL + "/Uploads/common/noimage.png";
            }
            else
            {
                ImagePath = HostURL + "/Uploads/Product/" + ProductCode + "/image.png";
            }
            return ImageURL;
        }*/

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

       // public async Task<ActionResult<Product>> PostProduct(Models.Product f, [FromServices] IWebHostEnvironment env)
        //public IActionResult PostProduct(Models.Product f, [FromServices] IWebHostEnvironment env)
        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct(Models.Product f, [FromServices] IWebHostEnvironment env)
        {
          if (_context.Product == null)
          {
              return Problem("DbContextProject.Product  is null.");
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
