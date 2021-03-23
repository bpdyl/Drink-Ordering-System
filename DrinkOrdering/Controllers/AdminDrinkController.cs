
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Models;
using DrinkOrdering.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Authorization;
using DrinkOrdering.Utilities;
namespace DrinkOrdering.Controllers
{
    /*   [Authorize(Roles =Permission.AdminUser)]*/
    public class AdminDrinkController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AdminDrinkController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _dbContext.Drinks.Include(s => s.Category).AsNoTracking().OrderBy(s => s.Name);
            var model = await PagingList.CreateAsync(query, 5, page);
            return View(model);
        }


        //[HttpGet] for search 
        //public async Task<IActionResult> Index(string productsearch)
        //{
        //    ViewData["GetProducts"] = productsearch;
        //    var query = from x in _dbContext.Drinks select x;

        //    if (!String.IsNullOrEmpty(productsearch))
        //    {
        //        query = query.Where(x => x.Name.Contains(productsearch));
        //    }
        //    return View(await query.AsNoTracking().ToListAsync());
        //}

        [HttpGet]
        public async Task<IActionResult> AddDrink()
        {
            ProductCategoryViewModel PAC = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = new Models.Drink()
            };
            return View(PAC);
        }



        [TempData]
        public string StatusMessage { get; set; }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDrink(ProductCategoryViewModel PAC)
        {

            var menuItemFromDb = await _dbContext.Drinks.FindAsync(PAC.Drinks.DrinkId);


            if (ModelState.IsValid)
            {
                var duplicate = _dbContext.Drinks.Include(s => s.Category).Where(s => s.Name == PAC.Drinks.Name && s.Category.CategoryId == PAC.Drinks.CategoryId);
                if (duplicate.Count() > 0)
                {
                    StatusMessage = "Error : Product Already exists under " + duplicate.First().Category.CategoryName + " Category, Please use Another Name";
                }
                else
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        byte[] p1 = null;
                        using (var fs1 = files[0].OpenReadStream())
                        {
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                        }
                        PAC.Drinks.ImageThumbnailUrl = p1;
                    }

                    _dbContext.Drinks.Add(PAC.Drinks);
                    await _dbContext.SaveChangesAsync();
                    TempData["message"] = PAC.Drinks.Name + " has been saved successfly";
                    return RedirectToAction(nameof(Index));
                }
            }
            ProductCategoryViewModel mv = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = new Models.Drink(),
                
            };
            StatusMessage = "Success: " + PAC.Drinks.Name + " has been saved successfully";
            return View(mv);
        }



        public async Task<IActionResult> EditDrink(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var products = await _dbContext.Drinks.SingleOrDefaultAsync(m => m.DrinkId == id);
            if (products == null)
            {
                return NotFound();
            }
            ProductCategoryViewModel PAC = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = products
            };
            return View(PAC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDrink(int id, ProductCategoryViewModel PAC)
        {
            if (ModelState.IsValid)
            {
                var drink = await _dbContext.Drinks.FindAsync(id);
                if (drink == null)
                {
                    return NotFound();
                }
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    drink.ImageThumbnailUrl = p1;
                }
                drink.Name = PAC.Drinks.Name;
                drink.ShortDescription = PAC.Drinks.ShortDescription;
                drink.LongDescription = PAC.Drinks.LongDescription;
                drink.Price = PAC.Drinks.Price;
                drink.IsPreferredDrink = PAC.Drinks.IsPreferredDrink;
                drink.InStock = PAC.Drinks.InStock;
                drink.CategoryId = PAC.Drinks.CategoryId;

                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ProductCategoryViewModel mv = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = new Models.Drink(),
                StatusMessage = StatusMessage
            };
            return View(mv);
        }

        public async Task<IActionResult> ViewDrink(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var drinks = await _dbContext.Drinks.SingleOrDefaultAsync(m => m.DrinkId == id);
            if (drinks == null)
            {
                return NotFound();
            }
            ProductCategoryViewModel PAC = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = drinks
            };
            return View(PAC);
        }

        public async Task<IActionResult> DeleteDrink(int id)
        {
            /*if (id ==null)
            {
                return NotFound();
            }*/
            var drink = await _dbContext.Drinks.SingleOrDefaultAsync(m => m.DrinkId == id);
            if (drink == null)
            {
                return NotFound();
            }
            _dbContext.Drinks.Remove(drink);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            /*return Json("delete");*/

        }
        public FileResult getImage(int id)
        {
            Drink drinks = _dbContext.Drinks.FirstOrDefault(p => p.DrinkId ==id );
            return File(drinks.ImageThumbnailUrl, drinks.ImageUrl);
        }
    }
}
