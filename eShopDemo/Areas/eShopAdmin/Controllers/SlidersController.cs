using eShopDemo.Data;
using eShopDemo.Models;
using eShopDemo.Utilities.File;
using eShopDemo.Utilities.Helpers;
using eShopDemo.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eShopDemo.Areas.eShopAdmin.Controllers
{
    [Area("eShopAdmin")]
    public class SlidersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public SlidersController(AppDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderVM sliderVM)
        {
            foreach (var photo in sliderVM.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Image type is not correct.");
                    return View();
                }
                if (!photo.CheckFileSize(32))
                {
                    ModelState.AddModelError("Photo", "Image size should not over 32 megabyte.");
                    return View();
                }
            }
            foreach (var photo in sliderVM.Photos)
            {
                string filename = Guid.NewGuid().ToString() + " " + photo.FileName;
                string path = Helper.GetFilePath(_webHost.WebRootPath, "images", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                Slider slider = new Slider
                {
                    Image = filename,
                    Description = sliderVM.Description,
                    Header = sliderVM.Header
                };

                await _context.Sliders.AddAsync(slider);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider)
        {
            if (id != slider.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!slider.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Image type is not correct.");
                        return View(slider);
                    }

                    if (!slider.Photo.CheckFileSize(32))
                    {
                        ModelState.AddModelError("Photo", "Image size should not over 32 megabyte.");
                        return View(slider);
                    }
                    string path = Helper.GetFilePath(_webHost.WebRootPath, "images", slider.Photo.Name);
                    Helper.DeleteFile(path);
                    string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;
                    string newPath = Helper.GetFilePath(_webHost.WebRootPath, "images", fileName);
                    using (FileStream stream = new FileStream(newPath, FileMode.Create))
                    {
                        await slider.Photo.CopyToAsync(stream);
                    }

                    slider.Image = fileName;
                    _context.Update(slider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            string path = Helper.GetFilePath(_webHost.WebRootPath, "images", slider.Image);
            Helper.DeleteFile(path);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }
}
