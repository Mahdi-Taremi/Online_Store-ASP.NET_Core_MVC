using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    public class OrdersController : Controller
    {

        private readonly DbContextProject _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(DbContextProject context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(User);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var orders = await _context.Order.Where(o => o.UserId == user.Id).ToListAsync();
            //var orders = await _context.Orders.Where(o => o.UserId == user.Id).ToListAsync();
            //var orders = await _context.Orders.Where(o => o.UserId == user.User).ToListAsync();
            return View(orders);
        }

        // ایجاد سفارش جدید
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,Price,Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                order.UserId = user.Id;
                order.OrderDate = DateTime.Now;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // حذف سفارش
        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }


}
