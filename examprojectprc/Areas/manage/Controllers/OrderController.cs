using examprojectpr.Core.Models;
using examprojectpr.Data.DAL;
using examprojectprc.PaginationHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace examprojectprc.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class OrderController : Controller
    {
        private readonly AppDbcontext _context;

        public OrderController(AppDbcontext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Orders.AsQueryable();
            
            PaginatedList<Order> paginatedOrder = PaginatedList<Order>.Create(query, page, 1);
            return View(paginatedOrder);

        }

        public async Task<IActionResult> Detail(int id)
        {
            Order order = await _context.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();

            return View(order);
        }

        public async Task<IActionResult> Accept(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();
            order.OrderStatus = Core.Enums.OrderStatus.Accepted;

            await _context.SaveChangesAsync();

            return RedirectToAction("index", "order");
        }

    }
}
