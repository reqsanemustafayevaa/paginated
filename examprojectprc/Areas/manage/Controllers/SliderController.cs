using examprojectpr.Business.Extentions;
using examprojectpr.Core.Models;
using examprojectpr.Data.DAL;
using Microsoft.AspNetCore.Mvc;

namespace examprojectprc.Areas.manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly AppDbcontext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbcontext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider>sliders=_context.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if(!ModelState.IsValid)
            {
                return View(slider);
            }

            if (slider.ImageFile != null)
            {

                
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "can only upload .jpeg or .png");
                    return View();
                }

                if (slider.ImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("ImageFile", "File size must be lower than 1mb");
                    return View();
                }




                slider.ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Required!");
                return View();
            }
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Update(int id)
        {
            Slider wantedslider= _context.Sliders.FirstOrDefault(x=> x.Id == id); 
            if (wantedslider == null)
            {
                return NotFound();
            }
            return View(wantedslider);
        }
        [HttpPost]
        public ActionResult Update(Slider slider)
        {
            Slider wantedslider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            
            if(!ModelState.IsValid)
            {
                return View(slider);
            }
            if (slider.ImageFile != null)
            {

                
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "can only upload .jpeg or .png");
                    return View();
                }

                if (slider.ImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("ImageFile", "File size must be lower than 1mb");
                    return View();
                }


                string deletepath = Path.Combine(_env.WebRootPath, "uploads/sliders", wantedslider.ImageUrl);
                if (System.IO.File.Exists(deletepath))
                {
                    System.IO.File.Delete(deletepath);
                }

                wantedslider.ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Required!");
                return View();
            }
            wantedslider.Title = slider.Title;
            wantedslider.Description = slider.Description;
            wantedslider.RedirecText = slider.RedirecText;
            wantedslider.RedirectUrl = slider.RedirectUrl;
           
            _context.SaveChanges();
            return RedirectToAction("Index");

            

        }
        public IActionResult Delete(int id)
        {
            Slider existslider=_context.Sliders.FirstOrDefault(s => s.Id == id);


            return View(existslider);
        }
        [HttpPost]
        public IActionResult Delete(Slider slider)
        {
            Slider existslider = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            _context.Sliders.Remove(existslider);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
