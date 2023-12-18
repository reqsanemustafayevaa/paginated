using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace examprojectprc.Areas.manage.Controllers
{
    [Area("manage")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public async Task<IActionResult> Index()
        {
            List<Genre>genres=await _genreService.GetAllAsync();
            return View(genres);
        }
        public   IActionResult CreateAsync()
        {
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            await _genreService.CreateAsync(genre);
            return RedirectToAction("Index");
        }
        public async  Task<IActionResult> UpdateAsync(int id)
        {
            var existgenre= await _genreService.GetByIdAsync(id);
            if (existgenre == null)
            {
                return View();
            }
            return View(existgenre);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _genreService.UpdateAsync(genre);
            return RedirectToAction("Index");
        }
       
        




    }
}
