using examprojectpr.Business.Services.Implementations;
using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace examprojectprc.Areas.manage.Controllers
{
    [Area("manage")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async Task<IActionResult> Index()
        {
            List<Author>authors=await _authorService.GetAllAsync();
            return View(authors);
        }
        public async Task< IActionResult> CreateAsync()
        {
            return View();

        }
        [HttpPost]
        public async Task< IActionResult> CreateAsync(Author author)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            await _authorService.CreateAsync(author);
            
            return RedirectToAction("index");
        }
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var existauthor = await _authorService.GetByIdAsync(id);
            if (existauthor == null)
            {
                return View();
            }
            return View(existauthor);
            
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Author author)
        {
            var existauthor = await _authorService.GetByIdAsync(author.Id);
            if (existauthor == null)
            {
                return View();
            }
            if(!ModelState.IsValid)
            {
                throw new NullReferenceException();
            }
            await _authorService.UpdateAsync(author);
            return RedirectToAction("index");
        }

    }
}
