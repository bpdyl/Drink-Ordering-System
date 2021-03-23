using DrinkOrdering.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using DrinkOrdering.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DrinkOrdering.Models.ViewModels;
using System.IO;

namespace DrinkOrdering.Controllers
{
    public class AdminDrinkModifiedController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IDrinkService _drinkService;
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public Drink drink { get; set; }
        public AdminDrinkModifiedController(ApplicationDbContext dbContext, IDrinkService drinkService, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _drinkService = drinkService;
            _categoryService = categoryService;
        }
        /*private readonly IDrinkService _drinkService;
        private readonly ICategoryService _categoryService;

        public AdminDrinkController(ICategoryService categoryService, IDrinkService drinkService)
        {
            _drinkService = drinkService;
            _categoryService = categoryService;
        }*/
        // GET: AdminController
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductCategoryViewModel PAC = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = new Models.Drink()
            };
            /*          drink = new Drink();*/
            if (id == null)
            {
                //create
                return View(PAC);
            }
            //update
            drink = _dbContext.Drinks.FirstOrDefault(u => u.DrinkId == id);
            if (drink == null)
            {
                return NotFound();
            }
            ProductCategoryViewModel PACEdit = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = new Models.Drink()
            };
            return View(PACEdit);
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(int? id, ProductCategoryViewModel PAC)
        {
            var menuItemFromDb = await _dbContext.Drinks.FindAsync(PAC.Drinks.DrinkId);
            if (ModelState.IsValid)
            {
                if (drink.DrinkId == 0)
                {
                    //create
                    var duplicate = _dbContext.Drinks.Include(s => s.Category).Where(
                        s => s.Name == PAC.Drinks.Name && s.Category.CategoryId == PAC.Drinks.CategoryId
                        );
                    if (duplicate.Count() > 0)
                    {
                        StatusMessage = "Error: Product Already exists under " + duplicate.First().Category.CategoryName + " Category, Please use Another Name";
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
                    }

                }
                else
                {
                    var dr = await _dbContext.Drinks.SingleOrDefaultAsync(m => m.DrinkId == id);
                    if (dr == null)
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
                        dr.ImageThumbnailUrl = p1;
                    }
                    _dbContext.Drinks.Update(drink);
                }
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ProductCategoryViewModel mv = new ProductCategoryViewModel()
            {
                CategoryList = await _dbContext.Categories.ToListAsync(),
                Drinks = new Models.Drink(),
                StatusMessage = StatusMessage
            };
            return View(mv);
        }

        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public IActionResult Create(Drink model)
                {
                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }

                    var dbModel = GetDataModel(model);
                    _drinkService.AddDrink(dbModel);

                    return RedirectToAction(nameof(Index));
                }
                private Drink GetDataModel(Drink model)
                {
                    var dbModel = new Drink
                    {
                        DrinkId = model.DrinkId,
                        Name = model.Name,
                        ShortDescription = model.ShortDescription,
                        LongDescription = model.LongDescription,
                        Price = model.Price,
                        ImageUrl = model.ImageUrl,
                        Category = model.Category,
                        ImageThumbnailUrl = model.ImageThumbnailUrl,
                        InStock = model.InStock,
                        IsPreferredDrink = model.IsPreferredDrink
                    };
                    return dbModel;
                }

                private DrinkViewModel GetViewModel(Drink dbModel)
                {
                    var model = new DrinkViewModel
                    {
                        DrinkId = dbModel.DrinkId,
                        Name = dbModel.Name,
                        ShortDescription = dbModel.ShortDescription,
                        Price = dbModel.Price,
                        ImageThumbnailUrl = dbModel.ImageThumbnailUrl,
                       };
                    return model;
                }

                [HttpGet]
                public IActionResult Edit(int id)
                {
                    var dbDrink = _drinkService.GetDrinkById(id);

                    var model = GetViewModel(dbDrink);

                    return View(model);
                }

                [HttpPost]
                public IActionResult Edit(DrinkViewModel model)
                {
                    var dbModel = GetDataModel(drink);
                    _drinkService.UpdateDrink(dbModel);

                    return RedirectToAction(nameof(Index));
                }*/

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _dbContext.Drinks.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var drinkFromDb = await _dbContext.Drinks.FirstOrDefaultAsync(u => u.DrinkId == id);
            if (drinkFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbContext.Drinks.Remove(drinkFromDb);
            await _dbContext.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion


    }
}