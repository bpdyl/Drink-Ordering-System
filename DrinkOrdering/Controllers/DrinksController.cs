using DrinkOrdering.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Services;
using DrinkOrdering.Models;
using DrinkOrdering.Models.ViewModels;
namespace DrinkOrdering.Controllers
{
    public class DrinksController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IDrinkService _drinkService;
       
        public DrinksController(ICategoryService categoryService, IDrinkService drinkService)
        {
            _categoryService = categoryService;
            _drinkService = drinkService;
        }
        public ViewResult List(string category)
        {
            DrinksListViewModel vm = new DrinksListViewModel();
            string _category = category;
            IEnumerable<Drink> drinks;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(category))
            {
                drinks = _drinkService.Drinks.OrderBy(p => p.DrinkId);
                currentCategory = "All drinks";
            }
            else
            {
                if (string.Equals("Alcoholic", _category, StringComparison.OrdinalIgnoreCase))
                    drinks = _drinkService.Drinks.Where(p => p.Category.CategoryName.Equals("Alcoholic")).OrderBy(p => p.Name);
                else
                    drinks = _drinkService.Drinks.Where(p => p.Category.CategoryName.Equals("Non-alcoholic")).OrderBy(p => p.Name);

                currentCategory = _category;
            }

            return View(new DrinksListViewModel
            {
                Drinks = drinks,
                CurrentCategory = currentCategory
            });
        }
    }
}
