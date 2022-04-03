using eShopDemo.Data;
using eShopDemo.Models;
using eShopDemo.Utilities.File;
using eShopDemo.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eShopDemo.Areas.eShopAdmin.Controllers
{
    [Area("eShopAdmin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Categories.Include(c => c.Parent);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Image,Photo,ParentId,Id,IsDeleted")] Category category)
        {
            if (!category.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is not correct");
                return View();
            }
            if (!category.Photo.CheckFileSize(32))
            {
                ModelState.AddModelError("Photo", "Image size should not be over 32 mb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + category.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await category.Photo.CopyToAsync(stream);
            }

            category.Image = fileName;

            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Image,Photo,ParentId,Id,IsDeleted")] Category category)
        {


            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoryy = await _context.Categories.FindAsync(id);
                    string path = Helper.GetFilePath(_env.WebRootPath, "images", category.Image);
                    Helper.DeleteFile(path);
                    string fileName = Guid.NewGuid().ToString() + "_" + category.Photo.FileName;
                    string NewPath = Helper.GetFilePath(_env.WebRootPath, "images", fileName);

                    using (FileStream stream = new FileStream(NewPath, FileMode.Create))
                    {
                        await category.Photo.CopyToAsync(stream);
                    }

                    category.Image = fileName;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            string path = Helper.GetFilePath(_env.WebRootPath, "images", category.Image);
            Helper.DeleteFile(path);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
