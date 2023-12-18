using examprojectpr.Business.Exceptions;
using examprojectpr.Business.Extentions;
using examprojectpr.Business.Services.Interfaces;
using examprojectpr.Core.Models;
using examprojectpr.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace examprojectprc.Areas.manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbcontext _context;
        public BookController(IBookService bookService, IWebHostEnvironment env, AppDbcontext context)
        {
            _bookService = bookService;
            _env = env;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books =await _bookService.GetAllAsync(); //
            return View(books);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _bookService.CreateAsync(book);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
           
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();


            Book existBook = await _bookService.GetByIdAsync(id);

            if (existBook == null) return NotFound();

            foreach (var item in existBook.BookTags)
            {
                existBook.TagIds.Add(item.TagId);
            }

            existBook.TagIds = existBook.BookTags.Select(x => x.TagId).ToList();

            return View(existBook);
        }

        [HttpPost]
        public IActionResult Update(Book book)
        {

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            Book existBook = _context.Books
                            .Include(x => x.BookTags)
                            .Include(x => x.BookImages)
                            .Include(x => x.BookTags).ThenInclude(x => x.Tag)
                            .FirstOrDefault(x => x.Id == book.Id);

            if (existBook == null) return NotFound();


            var destination = existBook.GetType().GetProperties();
            var source = book.GetType().GetProperties();

            for (int i = 0; i < destination.Length; i++)
            {
                destination[i].SetValue(existBook, source[i].GetValue(book));
            }

            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!");
                return View();
            }

            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!");
                return View();
            }


            existBook.BookTags.RemoveAll(bt => !book.TagIds.Contains(bt.TagId));
            foreach (var tagId in book.TagIds.Where(x => !existBook.BookTags.Any(bt => bt.TagId == x)))
            {
                BookTag bookTag = new BookTag
                {
                    Book = existBook,
                    TagId = tagId
                };
                existBook.BookTags.Add(bookTag);
            }


            if (book.BookPosterImagefiles != null)
            {
                if (book.BookPosterImagefiles.ContentType != "image/jpeg" && book.BookPosterImagefiles.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookPosterImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (book.BookPosterImagefiles.Length > 2097152)
                {
                    ModelState.AddModelError("BookPosterImagefiles", "File size must be lower than 2mb!");
                    return View();
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", book.BookPosterImagefiles),
                    IsPoster = true
                };

                existBook.BookImages.Add(bookImage);
            }

            if (book.BookHoverImageFiles != null)
            {
                if (book.BookHoverImageFiles.ContentType != "image/jpeg" && book.BookHoverImageFiles.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookHoverImageFiles", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (book.BookHoverImageFiles.Length > 2097152)
                {
                    ModelState.AddModelError("BookHoverImageFiles", "File size must be lower than 2mb!");
                    return View();
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", book.BookHoverImageFiles),
                    IsPoster = false
                };

                existBook.BookImages.Add(bookImage);
            }


            existBook.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsPoster == null);
            if (book.BookImageFiles != null)
            {
                foreach (var imageFile in book.BookImageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
                    {
                        ModelState.AddModelError("BookImageFiles", "File must be .png or .jpeg (.jpg)");
                        return View();
                    }
                    if (imageFile.Length > 2097152)
                    {
                        ModelState.AddModelError("BookImageFiles", "File size must be lower than 2mb!");
                        return View();
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };

                    _context.BookImages.Add(bookImage);
                    existBook.BookImages.Add(bookImage);
                }
            }



            existBook.Name = book.Name;
            existBook.Description = book.Description;
            existBook.CostPrice = book.CostPrice;
            existBook.SalePrice = book.SalePrice;
            existBook.Code = book.Code;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.IsAvailable = book.IsAvailable;
            existBook.Tax = book.Tax;
            existBook.AuthorId = book.AuthorId;
            existBook.GenreId = book.GenreId;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
