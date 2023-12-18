using examprojectpr.Data.DAL;
using examprojectprc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace examprojectprc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbcontext _context;
        public HomeController(AppDbcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders = _context.Sliders.ToList(),
                FeaturedBooks = _context.Books.Include(x => x.author).Include(x => x.BookImages).Where(x => x.IsFeatured == true).ToList(),
                NewBooks = _context.Books.Include(x => x.author).Include(x => x.BookImages).Where(x => x.IsNew == true).ToList(),
                BestSellerBooks = _context.Books.Include(x => x.author).Include(x => x.BookImages).Where(x => x.IsBestSeller == true).ToList(),
            };
            return View(homeViewModel);
        }
    }
}
