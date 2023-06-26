using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;

public class BasketService : IBasketService
{
   /* private readonly DbContextProject _context;

    public BasketService(DbContextProject context)
    {
        _context = context;
    }

    public async Task<Basket> GetBasketAsync(string userId)
    {
        return await _context.Basket.Include(b => b.Products)
                                      .FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task AddProductToBasketAsync(Product product, Basket basket)
    {
        if (basket.Products == null)
        {
            basket.Products = new List<Product>();
        }

        basket.Products.Add(product);
        await _context.SaveChangesAsync();
    }*/
}