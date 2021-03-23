using DrinkOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DrinkOrdering.Utilities;
namespace DrinkOrdering.Controllers
{
   /* [Authorize(Roles = Permission.AdminUser)]*/
    public class AdminCategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminCategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var cats = await _dbContext.Categories.ToListAsync();
            return View(cats);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category catmd)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(catmd);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catmd);
        }

        //edit 
        [HttpGet]
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cat = await _dbContext.Categories.FindAsync(id);
            Console.Write(cat.CategoryName);
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category cat)
        {
           /* if (cat.CategoryId == null)
            {
                return NotFound();
            }*/
            var catfromdb = await _dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == cat.CategoryId);
            if (catfromdb == null)
            {
                return NotFound();
            }
            System.Diagnostics.Debug.WriteLine(catfromdb.CategoryName);
            catfromdb.CategoryName = cat.CategoryName;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //view
        public async Task<IActionResult> ViewCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cat = await _dbContext.Categories.FindAsync(id);
            System.Diagnostics.Debug.WriteLine(cat.CategoryName);
            return View(cat);
        }

        //delete
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cat = await _dbContext.Categories.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(cat);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
