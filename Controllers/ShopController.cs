using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NuGet.Protocol;
using Online_Store_ASP.NET_Core_MVC.Models;
using Online_Store_ASP.NET_Core_MVC.Services;
using System.Text.Json;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{

    public class ShopController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductsController> _logger;
        public ShopController(IProductRepository productRepository, IFileUploadService fileUploadService, IDistributedCache cache, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
            _cache = cache;
            _logger = logger;
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = UsersRoles.ADMIN)]
        public IActionResult CreateProduct(Models.Product f)
        {
            f.pic_1 = _fileUploadService.Upload(f.UploadFile, "Uploads");
            _productRepository.Add(f);
            _productRepository.SaveChanges();

            return Ok("Add Product");
        }



        //[ValidateAntiForgeryToken]
        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = UsersRoles.ADMIN)]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var query = _productRepository.GetById(id);
            _productRepository.Remove(query);
            _productRepository.SaveChanges();
            return Ok("Delete Product");

        }

        // Test
        //[ValidateAntiForgeryToken]
        /*[HttpPost("BuyProduct")]
        [Authorize(Roles = UsersRoles.USER)]
        public string BuyProduct(int id)
        {
            var query = _productRepository.GetById(id);
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
            var query = _productRepository.GetById(Id);

            return ("Product Name : " + query.Name + " " + " and Price : " + query.Price);
        }

        [HttpGet("ShowWithoutRedis")]
        public async Task<IActionResult> ShowWithoutRedis(int Id)
        {
            var query = _productRepository.GetById(Id);
             await Task.Delay(3000);
            return Ok(query.ToJson());
        }

        [HttpGet("ShowWithRedis")]
        public async Task<IActionResult> Get(int Id)
        {
            var query = _productRepository.GetById(Id);
            string cacheKey =
                $"product:{Id}";

            var cachedProduct =
                await _cache.GetStringAsync(
                    cacheKey);

            if (!string.IsNullOrEmpty(
                    cachedProduct))
            {
                _logger.LogInformation(
                    "Redis Cache Hit {ProductId}",
                    Id);

                return Ok(query.ToJson());
            }

            _logger.LogInformation(
                "Redis Cache Miss {ProductId}",
                Id);

            await Task.Delay(3000);

            var productFromDb = query.ToJson();

            string json =
                JsonSerializer.Serialize(
                    productFromDb);

            await _cache.SetStringAsync(
                cacheKey,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(10)
                });
            return Ok();
        }
    }
}
